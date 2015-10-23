package com.srujan.bphc_id;

import android.content.ActivityNotFoundException;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ImageView;
import android.widget.TextView;

import java.io.InputStream;

public class MainActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener {
    //private MobileServiceClient mClient;
    //private MobileServiceTable<Students> mStudentsTable;
    //private MobileServiceTable<Seating> mSeatingTable;
    Personal personal=RegisterActivity.Identity;

    private static RecyclerView mRecyclerView;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.setDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);
        //LinearLayout llHeader=(LinearLayout)LayoutInflater.from(this).inflate(R.layout.nav_header_main, null);
        ImageView mImgPhoto=(ImageView)navigationView.findViewById(R.id.imageView);
        new DownloadImageTask(mImgPhoto).execute(personal.Photo);
        TextView txtName=(TextView)navigationView.findViewById(R.id.textView1);
        TextView txtEmail=(TextView)navigationView.findViewById(R.id.textView);
        txtName.setText(personal.Name);
        txtEmail.setText(personal.Email);
        //navigationView.addHeaderView(llHeader);

        mRecyclerView = (RecyclerView) findViewById(R.id.coursesView);
        mRecyclerView.setHasFixedSize(true);
        LinearLayoutManager llm = new LinearLayoutManager(this);
        mRecyclerView.setLayoutManager(llm);
        CoursesAdapter adapter = new CoursesAdapter(this);
        mRecyclerView.setAdapter(adapter);
    }

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

@Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_sync) {
            Intent i=new Intent(MainActivity.this,RegisterActivity.class);
            startActivity(i);
            finish();
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();

        if (id == R.id.sync) {
            Intent i=new Intent(MainActivity.this,RegisterActivity.class);
            startActivity(i);
            finish();
            return true;
        } else if (id == R.id.nav_bits) {
            Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://www.bits-pilani.ac.in/hyderabad/"));
            startActivity(browserIntent);
        } else if (id == R.id.nav_cms) {
            Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://cms.bits-hyderabad.ac.in"));
            startActivity(browserIntent);
        } else if (id == R.id.nav_swd) {
            Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://swd.bits-hyderabad.ac.in"));
            startActivity(browserIntent);
        } else if (id == R.id.nav_shoutboxx) {
            Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://www.facebook.com/groups/bphcshoutbox/"));
            startActivity(browserIntent);
        } else if (id == R.id.ratemyapp) {
            Uri uri = Uri.parse("market://details?id=" + getPackageName());
            Intent goToMarket = new Intent(Intent.ACTION_VIEW, uri);
            try {
                startActivity(goToMarket);
            } catch (ActivityNotFoundException e) {
                startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse("http://play.google.com/store/apps/details?id=" + getPackageName())));
            }
            return true;
        }

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
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
