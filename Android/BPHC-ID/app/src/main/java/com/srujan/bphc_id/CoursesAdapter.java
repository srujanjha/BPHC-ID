package com.srujan.bphc_id;

import android.content.Context;
import android.graphics.Color;
import android.support.v7.widget.CardView;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

/**
 * This adapter class populates cards related to categories in the Featured Categories section of the Home page.
 */
public class CoursesAdapter extends RecyclerView.Adapter<CoursesAdapter.CardViewHolder> {

    /**
     * Holder class to contain all the views required during populating content into it.
     */
    public static class CardViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener, View.OnLongClickListener {
        final CardView cardView;
        final TextView courseName;
        final TextView roomNo;
        final TextView T1Date;
        final TextView T2Date;
        final TextView Time;
        final LinearLayout llBack;
        //final ImageView Img1;
        //final ImageView Img2;

        //final ImageButton imgAlarm;

        CardViewHolder(View itemView) {
            super(itemView);
            //imgAlarm=(ImageButton)itemView.findViewById(R.id.imgAlarm);
            //Img1=(ImageView)itemView.findViewById(R.id.img1);
            //Img2=(ImageView)itemView.findViewById(R.id.img2);
            llBack=(LinearLayout)itemView.findViewById(R.id.llBack);
            cardView=(CardView)itemView.findViewById(R.id.cardCourses);
            courseName = (TextView) itemView.findViewById(R.id.txtCourseName);
            roomNo = (TextView) itemView.findViewById(R.id.txtRoomNo);
            T1Date = (TextView) itemView.findViewById(R.id.txtT1Date);
            T2Date = (TextView) itemView.findViewById(R.id.txtT2Date);
            Time = (TextView) itemView.findViewById(R.id.txtTime);
            itemView.setOnClickListener(this);
        }

        /* Interface for handling clicks - both normal and long ones. */
        public interface ClickListener {
            /**
             * Called when the view is clicked.
             *
             * @param v           view that is clicked
             * @param position    of the clicked item
             * @param isLongClick true if long click, false otherwise
             */
            void onClick(View v, int position, boolean isLongClick);

        }

        private ClickListener clickListener;

        /* Setter for listener. */
        public void setClickListener(ClickListener clickListener) {
            this.clickListener = clickListener;
        }

        @SuppressWarnings("deprecation")
        @Override
        public void onClick(View v) {
            // If not long clicked, pass last variable as false.
            clickListener.onClick(v, getPosition(), false);
        }

        @SuppressWarnings("deprecation")
        @Override
        public boolean onLongClick(View v) {

            // If long clicked, passed last variable as true.
            clickListener.onClick(v, getPosition(), true);
            return true;
        }
    }


    private List<Course> myCourseList = new ArrayList<>();
    private static Context mContext;
    int max=1;
    /**
     * Constructor to the Adapter
     * @param context: Context of the activity currently in view/active.
     */
    public CoursesAdapter(Context context) {
        mContext = context;
        System.out.println("CourseAdapter");
        Calendar c = Calendar.getInstance();
        Collections.sort(RegisterActivity.listCourses, new Comparator<Course>() {
            public int compare(Course o1, Course o2) {
                if (o1.T1date == null || o2.T1Date == null)
                    return 0;
                return o1.T1date.compareTo(o2.T1date);
            }
        });
        if(RegisterActivity.listCourses.get(RegisterActivity.listCourses.size()-1).T1date.compareTo(c)<0)
        {
            Collections.sort(RegisterActivity.listCourses, new Comparator<Course>() {
                public int compare(Course o1, Course o2) {
                    if (o1.T1date == null || o2.T1Date == null)
                        return 0;
                    return o1.T2date.compareTo(o2.T2date);
                }
            });
        }
        //int ar[]=new int[20];
        myCourseList=RegisterActivity.listCourses;
    }

    /**
     * Gets the total number of cards which is populated
     * @return Size of the Categories-List which is randomly generated
     */
    @Override
    public int getItemCount() {
        return myCourseList.size();
    }

    /**
     * Inflates the layout
     * @param viewGroup:View group object, which is inflated with the current layout.
     * @param i: Index of the object
     * @return CardViewHolder object of the current view.
     */
    @Override
    public CardViewHolder onCreateViewHolder(ViewGroup viewGroup, int i) {
        View v = LayoutInflater.from(viewGroup.getContext()).inflate(R.layout.card_courses, viewGroup, false);
        return new CardViewHolder(v);
    }
    int colorList[]={Color.BLUE,Color.YELLOW,Color.GREEN,Color.RED,Color.CYAN,Color.MAGENTA,Color.GRAY,};
    //AlarmManager alarmManager;
    //private PendingIntent pendingIntent;

    //boolean flag=true;
    /**
     * It binds content to the views.
     * @param cardViewHolder:Holder object which contains all the views which needs to be populated
     * @param i:Index of the card, which is being populated.
     */
    @Override
    public void onBindViewHolder(final CardViewHolder cardViewHolder,final int i) {
        System.out.println(myCourseList.get(i).courseCode + " " + myCourseList.get(i).T1date + " " + myCourseList.get(i).T2date);
        //cardViewHolder.cardView.setBackground(mContext.getResources().getDrawable(R.drawable.bits_tag, null));
        int color=myCourseList.get(i).T1date.get(Calendar.DAY_OF_MONTH) % colorList.length;
        cardViewHolder.llBack.setBackgroundColor(colorList[color]);
        cardViewHolder.courseName.setText(myCourseList.get(i).courseCode);
        cardViewHolder.T1Date.setText("T1-Date: "+myCourseList.get(i).T1Date);
        cardViewHolder.T2Date.setText("T2-Date: "+myCourseList.get(i).T2Date);
        cardViewHolder.Time.setText("Time: " + myCourseList.get(i).Time);
        if(RegisterActivity.Student)
        cardViewHolder.roomNo.setText("Room No.: "+myCourseList.get(i).Room);
        else cardViewHolder.roomNo.setVisibility(View.GONE);
        /*if(myCourseList.get(i).alarm==1)
            cardViewHolder.imgAlarm.setImageDrawable(mContext.getResources().getDrawable(R.drawable.ic_alarm_y,null));
        cardViewHolder.imgAlarm.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                alarmManager = (AlarmManager) mContext.getSystemService(mContext.ALARM_SERVICE);
                Intent myIntent = new Intent(mContext, AlarmReceiver.class);
                pendingIntent = PendingIntent.getBroadcast(mContext, 0, myIntent, 0);
                Bundle bundle = new Bundle();
                bundle.putString("Course",myCourseList.get(i).courseCode);
                bundle.putString("Room",myCourseList.get(i).Room);
                if(myCourseList.get(i).alarm==1)
                {myCourseList.get(i).alarm=0;
                    if (alarmManager!= null) {
                        alarmManager.cancel(pendingIntent);
                    }
                }
                else { myCourseList.get(i).alarm=1;
                    alarmManager.set(AlarmManager.RTC_WAKEUP, Calendar.getInstance().getTimeInMillis()-1000 * 60 *2,pendingIntent);//myCourseList.get(i).T1date.getTimeInMillis() - 1000 * 60 * 60 * 30, pendingIntent);
                    alarmManager.set(AlarmManager.RTC_WAKEUP, Calendar.getInstance().getTimeInMillis()+1000 * 60 *2,pendingIntent);//myCourseList.get(i).T2date.getTimeInMillis() - 1000 * 60 * 60 * 30, pendingIntent);
                }
            }
        });*/
        cardViewHolder.setClickListener(new CardViewHolder.ClickListener() {
            @Override
            public void onClick(View v, int pos, boolean isLongClick) {
                if (isLongClick) {
                    // View v at position pos is long-clicked.
                } else {

                }
            }
        });
    }

}