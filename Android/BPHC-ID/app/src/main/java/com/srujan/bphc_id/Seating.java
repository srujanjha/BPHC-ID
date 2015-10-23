package com.srujan.bphc_id;

/**
 * Created by Srujan on 10/14/2015.
 */
public class Seating {
    public String Id;
    public String course;
    public String t1date;
    public String t2date;
    public String time;
    public String room;
    public String students;
    public Boolean compre;
    public Seating(String c1, String t1, String t2, String t, String r, String stus,Boolean co ) {
        course = c1;
        t1date = t1;
        t2date = t2;
        time = t;
        room = r;
        students = stus;
        compre = co;
    }
}
