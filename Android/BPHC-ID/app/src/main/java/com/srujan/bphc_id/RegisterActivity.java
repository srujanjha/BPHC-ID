package com.srujan.bphc_id;

import android.content.Context;
import android.content.Intent;
import android.content.IntentSender;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.Scopes;
import com.google.android.gms.common.SignInButton;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.common.api.Scope;
import com.google.android.gms.plus.Plus;
import com.google.android.gms.plus.model.people.Person;
import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.http.ServiceFilterResponse;
import com.microsoft.windowsazure.mobileservices.table.MobileServiceTable;
import com.microsoft.windowsazure.mobileservices.table.TableQueryCallback;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.MalformedURLException;
import java.util.ArrayList;
import java.util.List;

public class RegisterActivity extends AppCompatActivity implements
        GoogleApiClient.ConnectionCallbacks,
        GoogleApiClient.OnConnectionFailedListener,
        View.OnClickListener {

    private static final int RC_SIGN_IN = 0;
    private static final String TAG = "RegisterActivity";
    private GoogleApiClient mGoogleApiClient;
    private boolean mIsResolving = false;
    public static List<Course> listCourses = new ArrayList<>();
    Students student = null;
    public static Personal Identity;
    private boolean mShouldResolve = false;
    String ID = "", Name = "", Email = "", personPhoto;
    private EditText mName, mEmail, mID;
    private TextView mtxtID;
    private ImageView mImgPhoto;
    private SignInButton signInButton;
    private Button btnNext, signOutButton;
    private MobileServiceClient mClient;
    private MobileServiceTable<Students> mStudentsTable;
    private MobileServiceTable<Seating> mSeatingTable;
    private MobileServiceTable<Invigilators> mInvigilatorsTable;
    private MobileServiceTable<Teachers> mTeachersTable;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);
        mGoogleApiClient = new GoogleApiClient.Builder(this)
                .addConnectionCallbacks(this)
                .addOnConnectionFailedListener(this)
                .addApi(Plus.API)
                .addScope(new Scope(Scopes.PROFILE))
                .addScope(new Scope(Scopes.EMAIL))
                .build();
        signInButton = (SignInButton) findViewById(R.id.sign_in_button);
        signInButton.setOnClickListener(this);

        signOutButton = (Button) findViewById(R.id.sign_out_button);
        signOutButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onSignOutClicked();
            }
        });
        btnNext = (Button) findViewById(R.id.btnNext);
        btnNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Next();
            }
        });
        mName = (EditText) findViewById(R.id.txtName);
        mID = (EditText) findViewById(R.id.txtID);
        mtxtID = (TextView) findViewById(R.id.txtViewID);
        mEmail = (EditText) findViewById(R.id.txtEmail);
        mImgPhoto = (ImageView) findViewById(R.id.imgPhoto);
        listCourses.clear();
    }

    int index = 0;
    int count = 0;
    public static Boolean Student=true;
    AsyncTask<Void, Void, Void> getStudentsList, getCourseList,getInvigilatorsList;

    private void downloadData() {
        ProgressBar prg = (ProgressBar) findViewById(R.id.prgBar);
        prg.setVisibility(View.VISIBLE);
        listCourses.clear();
        btnNext.setEnabled(false);
        try {
            mClient = new MobileServiceClient(
                    "https://bphc-id.azure-mobile.net/",
                    "baAsGGdkVmCtxhKBpnhDHOANERJLEG92",
                    this);
            mStudentsTable = mClient.getTable(Students.class);
            mSeatingTable = mClient.getTable(Seating.class);
            mInvigilatorsTable = mClient.getTable(Invigilators.class);
            mTeachersTable = mClient.getTable(Teachers.class);
        } catch (MalformedURLException e) {
            Snackbar.make(signInButton, "There was an error creating the Mobile Service. Verify the URL", Snackbar.LENGTH_LONG).show();
        }
        System.out.println(mClient);
        System.out.println(Name.toUpperCase());
        getInvigilatorsList = new AsyncTask<Void, Void, Void>() {
            @Override
            protected Void doInBackground(Void... params) {
                try {
                    mTeachersTable.top(1000).execute(new TableQueryCallback<Teachers>() {
                        @Override
                        public void onCompleted(List<Teachers> result, int arg1, Exception arg2, ServiceFilterResponse arg3) {
                            if (arg2 == null) {
                                if (!result.isEmpty()) {
                                    System.out.println(result.size());
                                    if (result.size() > 0) {
                                        for (Teachers stu : result) {
                                            System.out.println(stu.name);
                                            if(stu.name.toUpperCase().contains(Name.toUpperCase())) {
                                                Student = false;
                                                Identity = new Personal(Name, ID, Email, personPhoto);
                                                System.out.println(stu.name + " " + stu.email);
                                                runOnUiThread(new Runnable() {
                                                    @Override
                                                    public void run() {
                                                        System.out.println("Executed");
                                                        getCourseList = new AsyncTask<Void, Void, Void>() {
                                                            @Override
                                                            protected Void doInBackground(Void... params) {
                                                                try {
                                                                    mInvigilatorsTable.top(1000).execute(new TableQueryCallback<Invigilators>() {
                                                                        @Override
                                                                        public void onCompleted(List<Invigilators> result, int arg1,
                                                                                                Exception arg2, ServiceFilterResponse arg3) {
                                                                            //index++;
                                                                            if (arg2 == null) {
                                                                                System.out.println(result.size());
                                                                                if (!result.isEmpty()) {
                                                                                    for (Invigilators seat : result) {
                                                                                        for (String name : seat.names.split(":"))
                                                                                            if (Name.toUpperCase().contains(name.toUpperCase())) {
                                                                                                listCourses.add(new Course(seat.course, seat.t1date, seat.t2date, seat.time, 0,seat.course_name));
                                                                                                System.out.println(seat.course+" "+seat.course_name);
                                                                                            }
                                                                                    }
                                                                                } else {
                                                                                    System.out.println("Error" + result);
                                                                                }
                                                                            }
                                                                            System.out.println(index + " " + count);
                                                                            if (index == count) {
                                                                                runOnUiThread(new Runnable() {
                                                                                    @Override
                                                                                    public void run() {
                                                                                        ProgressBar prg = (ProgressBar) findViewById(R.id.prgBar);
                                                                                        prg.setVisibility(View.GONE);
                                                                                        btnNext.setEnabled(true);
                                                                                    }
                                                                                });
                                                                            }
                                                                        }
                                                                    });
                                                                    //}
                                                                } catch (Exception exception) {
                                                                    Snackbar.make(signInButton, exception.toString(), Snackbar.LENGTH_LONG).show();
                                                                }
                                                                return null;
                                                            }
                                                        }.execute();
                                                    }
                                                });
                                            break;}
                                        }
                                    }
                                } else {
                                    System.out.println("Error" + result);
                                }
                            }
                        }
                    });
                }catch(Exception e){System.out.println("Error" + e.toString());}
                return null;
            }
        }.execute();
        getStudentsList = new AsyncTask<Void, Void, Void>() {
            @Override
            protected Void doInBackground(Void... params) {
                try {
                    mStudentsTable.where().field("name").eq(Name.toUpperCase()).execute(new TableQueryCallback<Students>() {
                        @Override
                        public void onCompleted(List<Students> result, int arg1, Exception arg2, ServiceFilterResponse arg3) {
                            if (arg2 == null) {
                                if (!result.isEmpty()) {
                                    System.out.println(result.size());
                                    if (result.size() == 0) {
                                        System.out.println("Not a Student");
                                    } else {
                                        for (Students stu : result) {
                                            student = stu;
                                            Student=true;
                                            ID = stu.campus_id;
                                            Identity = new Personal(Name, ID, Email, personPhoto);
                                            System.out.println(stu.campus_id + " " + stu.course_id + " " + stu.name + " " + stu.Id);
                                            runOnUiThread(new Runnable() {
                                                @Override
                                                public void run() {
                                                    mID.setVisibility(View.VISIBLE);
                                                    mtxtID.setVisibility(View.VISIBLE);
                                                    mID.setText(ID);
                                                    System.out.println("Executed");
                                                    getCourseList = new AsyncTask<Void, Void, Void>() {

                                                        @Override
                                                        protected Void doInBackground(Void... params) {
                                                            try {
                                                                String[] ar = student.course_id.split(",");
                                                                System.out.println(student.course_id);
                                                                index = 0;
                                                                count = ar.length;
                                                                for (String course : ar) {
                                                                    System.out.println("Getting results for " + course);

                                                                    mSeatingTable.where().field("course").eq(course).execute(new TableQueryCallback<Seating>() {
                                                                        @Override
                                                                        public void onCompleted(List<Seating> result, int arg1,
                                                                                                Exception arg2, ServiceFilterResponse arg3) {
                                                                            index++;
                                                                            if (arg2 == null) {
                                                                                System.out.println(result.size());
                                                                                if (!result.isEmpty()) {
                                                                                    for (Seating seat : result) {
                                                                                        if (seat.students.contains(ID)) {
                                                                                            listCourses.add(new Course(seat.course, seat.t1date, seat.t2date, seat.time, seat.room, 0));
                                                                                            System.out.println(seat.course + " " + seat.students);
                                                                                        }
                                                                                    }
                                                                                } else {
                                                                                    System.out.println("Error" + result);
                                                                                }
                                                                            }
                                                                            System.out.println(index + " " + count);
                                                                            if (index == count) {
                                                                                runOnUiThread(new Runnable() {
                                                                                    @Override
                                                                                    public void run() {
                                                                                        ProgressBar prg = (ProgressBar) findViewById(R.id.prgBar);
                                                                                        prg.setVisibility(View.GONE);
                                                                                        btnNext.setEnabled(true);
                                                                                    }
                                                                                });
                                                                            }
                                                                        }
                                                                    });
                                                                }
                                                            } catch (Exception exception) {
                                                                Snackbar.make(signInButton, exception.toString(), Snackbar.LENGTH_LONG).show();
                                                            }
                                                            return null;
                                                        }
                                                    }.execute();
                                                }
                                            });
                                        }
                                    }
                                }else{
                                    System.out.println("Error" + result);
                                }
                            }
                        }
                    });
                } catch (Exception exception) {
                    Snackbar.make(signInButton, exception.toString(), Snackbar.LENGTH_LONG).show();
                }
                return null;
            }
        }.execute();
    }

    private boolean isNetworkAvailable() {
        ConnectivityManager connectivityManager
                = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
        return activeNetworkInfo != null && activeNetworkInfo.isConnected();
        //return false;
    }

    @Override
    protected void onStart() {
        super.onStart();
        if (isNetworkAvailable())
            mGoogleApiClient.connect();
        else {
            if (!getBaseContext().getFileStreamPath("Notification.csv").exists()) {
                mGoogleApiClient.connect();
                Snackbar.make(signInButton, "Please connect to internet!", Snackbar.LENGTH_LONG).show();
                return;
            }
            BufferedReader reader = null;
            try {
                reader = new BufferedReader(new InputStreamReader(openFileInput("Notification.csv")));
                String mLine = reader.readLine();
                String ar[] = mLine.split(",");
                if(ar.length==3){Student=false;Identity = new Personal(ar[0], "", ar[1], ar[2]);}
                    else {Student=true;Identity = new Personal(ar[0], ar[1], ar[2], ar[3]);}
                    mLine = reader.readLine();
                while (mLine != null) {
                    ar = mLine.split(",");
                    if(Student)listCourses.add(new Course(ar[0], ar[1], ar[2], ar[3], ar[4],Integer.parseInt(ar[5])));
                    else listCourses.add(new Course(ar[0], ar[1], ar[2], ar[3], Integer.parseInt(ar[4]),ar[5]));
                    mLine = reader.readLine();
                }
                reader.close();
            } catch (IOException e) {
                System.out.println(e);
            } finally {
                if (reader != null) {
                    try {
                        reader.close();
                        Intent i = new Intent(RegisterActivity.this, MainActivity.class);
                        startActivity(i);
                        finish();
                    } catch (IOException e) {
                        System.out.println(e);
                    }
                }
            }
        }

    }
    @Override
    protected void onStop() {
        super.onStop();
        mGoogleApiClient.disconnect();
    }

    public void onConnectionFailed(ConnectionResult connectionResult) {
        Log.d(TAG, "onConnectionFailed:" + connectionResult);

        if (!mIsResolving && mShouldResolve) {
            if (connectionResult.hasResolution()) {
                try {
                    connectionResult.startResolutionForResult(this, RC_SIGN_IN);
                    mIsResolving = true;
                } catch (IntentSender.SendIntentException e) {
                    Log.e(TAG, "Could not resolve ConnectionResult.", e);
                    mIsResolving = false;
                    mGoogleApiClient.connect();
                }
            } else {
                showErrorDialog(connectionResult);
            }
        } else {
            showSignedOutUI();
        }
    }

    @Override
    public void onClick(View v) {
        if (v.getId() == R.id.sign_in_button) {
            onSignInClicked();
        } else if (v.getId() == R.id.sign_out_button) {
            onSignOutClicked();
        } else if (v.getId() == R.id.btnNext) {
            Next();
        }
    }

    private void onSignInClicked() {
        mShouldResolve = true;
        mGoogleApiClient.connect();
        // Show a message to the user that we are signing in.
        Snackbar.make(signInButton, R.string.signing_in, Snackbar.LENGTH_SHORT).show();
    }

    private void onSignOutClicked() {
        // Clear the default account so that GoogleApiClient will not automatically
        // connect in the future.
        if (mGoogleApiClient.isConnected()) {
            Plus.AccountApi.clearDefaultAccount(mGoogleApiClient);
            mGoogleApiClient.disconnect();
        }
        showSignedOutUI();
        try {
            getCourseList.cancel(true);
            getStudentsList.cancel(true);
            listCourses.clear();
            LinearLayout liner = (LinearLayout) findViewById(R.id.llInfo);
            liner.setVisibility(View.GONE);
            liner = (LinearLayout) findViewById(R.id.llStart);
            liner.setVisibility(View.VISIBLE);
        } catch (Exception e) {
        }
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        Log.d(TAG, "onActivityResult:" + requestCode + ":" + resultCode + ":" + data);

        if (requestCode == RC_SIGN_IN) {
            if (resultCode != RESULT_OK) {
                mShouldResolve = false;
            }

            mIsResolving = false;
            mGoogleApiClient.connect();
        }
    }

    @Override
    public void onConnected(Bundle bundle) {
        Log.d(TAG, "onConnected:" + bundle);
        mShouldResolve = false;

        if (Plus.PeopleApi.getCurrentPerson(mGoogleApiClient) != null) {
            Person currentPerson = Plus.PeopleApi.getCurrentPerson(mGoogleApiClient);
            Name = currentPerson.getDisplayName();
            personPhoto = currentPerson.getImage().getUrl();
            Email = Plus.AccountApi.getAccountName(mGoogleApiClient);
            if (!Email.endsWith("@hyderabad.bits-pilani.ac.in")) {
                if (mGoogleApiClient.isConnected()) {
                    Plus.AccountApi.clearDefaultAccount(mGoogleApiClient);
                    mGoogleApiClient.disconnect();
                }
                showSignedOutUI();
                Snackbar.make(signInButton, "Please use your BITS-Official ID", Snackbar.LENGTH_LONG).show();
                return;
            }
            Name=("Aruna Malapati");
            Email =("arunam@hyderabad.bits-pilani.ac.in");
            mName.setText(Name);
            downloadData();
            mEmail.setText(Email);
            new DownloadImageTask(mImgPhoto).execute(personPhoto);
            LinearLayout liner = (LinearLayout) findViewById(R.id.llInfo);
            liner.setVisibility(View.VISIBLE);
            liner = (LinearLayout) findViewById(R.id.llStart);
            liner.setVisibility(View.GONE);
        }
        Log.d(TAG, "onConnected:" + mGoogleApiClient);
        showSignedInUI();
    }

    public void Next() {
        System.out.println("Done");
        Thread t1 = new Thread(new Runnable() {
            @Override
            public void run() {
                BufferedWriter wr = null;
                try {
                    wr = new BufferedWriter(new OutputStreamWriter(openFileOutput("Notification.csv", MODE_PRIVATE)));
                    if(Student)wr.write(Name + "," + Identity.ID + "," + Email + "," + personPhoto);
                    else wr.write(Name + "," + Email + "," + personPhoto);
                    System.out.println(Name + "," + Identity.ID + "," + Email + "," + personPhoto);
                    wr.newLine();
                    for (Course c : listCourses) {
                        if(Student)wr.write(c.courseCode + "," + c.T1Date + "," + c.T2Date + "," + c.Time + "," + c.Room+","+c.alarm);
                        else wr.write(c.courseCode + "," + c.T1Date + "," + c.T2Date + "," + c.Time + "," + c.alarm + "," +c.courseName);
                        System.out.println(c.courseCode + "," + c.T1Date + "," + c.T2Date + "," + c.Time + "," + c.Room+","+c.alarm);
                        wr.newLine();
                    }
                    wr.close();
                } catch (IOException e) {
                    System.out.println(e);
                } finally {
                    try {
                        if (wr != null) wr.close();
                        Intent i = new Intent(RegisterActivity.this, MainActivity.class);
                        startActivity(i);
                        finish();
                    } catch (IOException e) {
                        System.out.println(e);
                    }
                }
            }
        });
        t1.run();
    }

    @Override
    public void onConnectionSuspended(int i) {

    }

    private void showErrorDialog(ConnectionResult result) {
        Snackbar.make(signInButton, result.toString(), Snackbar.LENGTH_LONG).show();
    }

    private void showSignedInUI() {
        signInButton.setVisibility(View.GONE);
        signOutButton.setVisibility(View.VISIBLE);
        Snackbar.make(signInButton, "Signed In", Snackbar.LENGTH_LONG).show();
    }

    private void showSignedOutUI() {
        Snackbar.make(signInButton, "Signed Out", Snackbar.LENGTH_LONG).show();
        signInButton.setVisibility(View.VISIBLE);
        signOutButton.setVisibility(View.GONE);
        mName.setText("");
        mEmail.setText("");
        mID.setText("");
        mImgPhoto.setImageBitmap(null);
        try {
            getCourseList.cancel(true);
            getStudentsList.cancel(true);
            listCourses.clear();
            LinearLayout liner = (LinearLayout) findViewById(R.id.llInfo);
            liner.setVisibility(View.GONE);
            liner = (LinearLayout) findViewById(R.id.llStart);
            liner.setVisibility(View.VISIBLE);
        } catch (Exception e) {
        }
    }

    private class DownloadImageTask extends AsyncTask<String, Void, Bitmap> {
        ImageView bmImage;

        public DownloadImageTask(ImageView bmImage) {
            this.bmImage = bmImage;
        }

        protected Bitmap doInBackground(String... urls) {
            String urldisplay = urls[0];
            Bitmap mIcon11 = null;
            try {
                InputStream in = new java.net.URL(urldisplay).openStream();
                mIcon11 = BitmapFactory.decodeStream(in);
            } catch (Exception e) {
                Log.e("Error", e.getMessage());
                e.printStackTrace();
            }
            return mIcon11;
        }

        protected void onPostExecute(Bitmap result) {
            bmImage.setImageBitmap(result);
        }
    }
}