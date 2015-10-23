package com.srujan.bphc_id;

/**
 * Created by Srujan on 10/19/2015.
 */
public class Invigilators {
    public String Id;
    public String course;
    public String course_name;
    public String t1date;
    public String t2date;
    public String time;
    public String names;
    public Invigilators(String c1,String c2, String t1, String t2, String t, String name ) {
        course = c1;
        course_name=c2;
        t1date = t1;
        t2date = t2;
        time = t;
        names=name;
    }
}
