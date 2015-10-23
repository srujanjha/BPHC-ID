package com.srujan.bphc_id;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.GregorianCalendar;


/**
 * Created by Srujan on 9/25/2015.
 */
public class Course {
    public String courseCode = "",courseName="",Room="",T1Date = "",T2Date = "", Time = "";
    public int colorCode=0;public int alarm=0;
    public Calendar T1date=new GregorianCalendar(),T2date=new GregorianCalendar();
    public Course(String c1, String c2,String c3,String c4,String c5,int c6)
    {
        String ss=c4.split("-")[0];
        int hh=Integer.parseInt(ss.substring(0,ss.indexOf(".")));
        int mm=Integer.parseInt(ss.substring(ss.indexOf(".")+1));
        if(c4.charAt(c4.length()-3)=='P')hh+=12;
        SimpleDateFormat dateFormat = new SimpleDateFormat("dd-MMM-yyyy");
        try {
            if(!c2.equals("")) {
                T1date.setTime(dateFormat.parse(c2));
                T1date.set(T1date.get(Calendar.YEAR), T1date.get(Calendar.MONTH), T1date.get(Calendar.DAY_OF_MONTH), hh, mm);
            }
            if(!c3.equals("")) {
                T2date.setTime(dateFormat.parse(c3));
                T2date.set(T2date.get(Calendar.YEAR), T2date.get(Calendar.MONTH), T2date.get(Calendar.DAY_OF_MONTH), hh, mm);
            }
        } catch (ParseException e) {
            e.printStackTrace();
        }

        courseCode = c1;T1Date=c2;T2Date=c3;Time=c4;Room=c5;alarm=c6;
    }
    public Course(String c1, String c2,String c3,String c4,int c6,String c5)
    {
        String ss=c4.split("-")[0];
        int hh=Integer.parseInt(ss.substring(0,ss.indexOf(".")));
        int mm=Integer.parseInt(ss.substring(ss.indexOf(".")+1));
        if(c4.charAt(c4.length()-3)=='P')hh+=12;
        SimpleDateFormat dateFormat = new SimpleDateFormat("dd/MM");
        Calendar date=Calendar.getInstance();
        try {
            if(!c2.equals("")) {
                T1date.setTime(dateFormat.parse(c2));
                T1date.set(date.get(Calendar.YEAR), T1date.get(Calendar.MONTH), T1date.get(Calendar.DAY_OF_MONTH), hh, mm);
            }
            if(!c3.equals("")) {
                T2date.setTime(dateFormat.parse(c3));
                T2date.set(date.get(Calendar.YEAR), T2date.get(Calendar.MONTH), T2date.get(Calendar.DAY_OF_MONTH), hh, mm);
            }
        } catch (ParseException e) {
            e.printStackTrace();
        }
        courseCode = c1;T1Date=c2;T2Date=c3;Time=c4;courseName=c5;alarm=c6;
    }
    @Override
    public String toString()
    {
        return courseCode;
    }
}
