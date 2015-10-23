package com.srujan.bphc_id;

/**
 * Created by Srujan on 10/14/2015.
 */
public class Students {
        public String Id;
        public String campus_id;
        public String course_id;
        public String name;
        public Students(String cid,String ccid,String name1)
        {
            course_id = ccid;campus_id = cid;name = name1;
        }
}
