package com.task.common;



/**
 * <p>
 * Title:
 * </p>
 * <p>
 * Description:
 * </p>
 * <p>
 * Copyright: Copyright (c) 2004
 * </p>
 * <p>
 * Company:
 * </p>
 * 
 * @author Miller
 * @version 0.6
 */
import java.util.Calendar;
import java.util.GregorianCalendar;
import java.util.Date;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.text.ParseException;

public class DateUtil {
	
    private final static String shortDateFormatPatten = "yyyy/MM/dd";
    private final static String longDateFormatPatten = "yyyy/MM/dd HH:mm:ss:SS";
    private final static String shortDateFormatPatten2="yyyy-MM-dd";
    private final static String shortDateFormatPatten3="yyyy-MM-dd HH:mm:ss";
    private final static String shortDateFormatPatten4="yyyyMMddHHmmss";
    private final static String yearFormatPatten = "yyyy";
    private final static String hourFormatPatten = "yyyy-MM-dd HH";
   
    private final static String DateFormatPatten2="yyyy-MM";
    private final static String DateFormatPatten3="HH:mm";
    private final static String DateFormatPatten4="yyyy-MM-dd HH:mm";
    private final static String DateFormatPatten5="yyyyMMdd";
    private final static String DateFormatPatten6="yyyy-MM-dd HH:mm:ss";

    public static Date getCurrentDate() {
        GregorianCalendar ca = new GregorianCalendar();
        return ca.getTime();
    }

    public static String getCurrentDateAsString(String reqFormatPatten) {
        String formatPatten = shortDateFormatPatten;
        if (reqFormatPatten != null)
            formatPatten = reqFormatPatten;
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten);
        return dateFormat.format(getCurrentDate());
    }

    public static Date parse1(String dateString) throws ParseException {
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten);
        return dateFormat.parse(dateString);
    }
    public static Date parse2(String dateString) throws ParseException {
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten2);
        return dateFormat.parse(dateString);
    }
    public static Date parse3(String dateString) throws ParseException {
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten3);
        return dateFormat.parse(dateString);
    }
    
    public static Date parse4(String dateString) throws ParseException {
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten4);
        return dateFormat.parse(dateString);
    }
    public static Date parse5(String dateString) throws ParseException {
        DateFormat dateFormat = new SimpleDateFormat(yearFormatPatten);
        return dateFormat.parse(dateString);
    }
    
    public static Date parse6(String dateString) throws ParseException {
        DateFormat dateFormat = new SimpleDateFormat(hourFormatPatten);
        return dateFormat.parse(dateString);
    }
    ////////////////////////
    
    public static String formatYear(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(yearFormatPatten);
        return dateFormat.format(date);
    }
    
    public static String formatHour(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(hourFormatPatten);
        return dateFormat.format(date);
    }
    
    public static String format(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten);
        return dateFormat.format(date);
    }
    
    public static String format(Date date,String format) {
        DateFormat dateFormat = new SimpleDateFormat(format);
        return dateFormat.format(date);
    }
    
    public static String formatDateYYYYMM(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(DateFormatPatten2);
        return dateFormat.format(date);
    }
    
    public static String formatTime(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(DateFormatPatten3);
        return dateFormat.format(date);
    }
    
    public static String formatTimeHHMM(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(DateFormatPatten4);
        return dateFormat.format(date);
    }
    
    public static String formatDate3(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten3);
        return dateFormat.format(date);
    }
    
    public static String formatDate4(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten4);
        return dateFormat.format(date);
    }
    
    public static String formatDate5(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(DateFormatPatten5);
        return dateFormat.format(date);
    }
    
    public static String formatDate6(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(DateFormatPatten6);
        return dateFormat.format(date);
    }
 ////////////----------------------
    public static String formatshort(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten2);
        return dateFormat.format(date);
    }
    public static String format(String dateStr, int offset) {
    	Date date = new Date(dateStr);
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	c.add(Calendar.DATE,offset);
        DateFormat dateFormat = new SimpleDateFormat(shortDateFormatPatten);
        return dateFormat.format(c.getTime());
    }
    
    public static String formatLong(Date date) {
        DateFormat dateFormat = new SimpleDateFormat(longDateFormatPatten);
        return dateFormat.format(date);
    }
    
    public static String newyear(Date date) {
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	String year = TimeFull(String.valueOf(c.get(Calendar.YEAR)));
    	String newyear=year.substring(2,4);
    	return newyear;
    }
    
    public static String year(Date date) {
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	String year = TimeFull(String.valueOf(c.get(Calendar.YEAR)));
    	//String newyear=year.substring(2,4);
    	return year;
    }
    
    public static String shortyear(Date date) {
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	String year = TimeFull(String.valueOf(c.get(Calendar.YEAR))); 
    	String newyear=year.substring(2,4);
    	return newyear;
    }
    
    public static String month(Date date) {
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	String month = TimeFull(String.valueOf(c.get(Calendar.MONTH)+1));
    	return month;
    }

    public static String minute(Date date) {
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	String minute = TimeFull(String.valueOf(c.get(Calendar.MINUTE)));
    	return minute;
    }
    
    public static String second(Date date) {
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	String second = TimeFull(String.valueOf(c.get(Calendar.SECOND)));
    	return second;
    }
    public static String misecond(Date date) {
    	
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	String	misecond = TimeFull2(String.valueOf(c.get(Calendar.MILLISECOND)));
        return misecond;
    }
    public static String hour(Date date) {
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	String hour = TimeFull(String.valueOf(c.get(Calendar.HOUR)));
    	return hour;
    }
    public static String date(Date date) {
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	String day = TimeFull(String.valueOf(c.get(Calendar.DATE)));
    	return day;
    }
    
    public static Date dateOffset(Date date, int offset) {
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	c.add(Calendar.DATE,offset);
        return c.getTime();
    }
    
    public static Date yearOffset(Date date, int offset){
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	c.add(Calendar.YEAR,offset);
        return c.getTime();
    }
    
    public static Date monthOffset(Date date, int offset){
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	c.add(Calendar.MONTH,offset);
        return c.getTime();
    }
    
    public static Date hourOffset(Date date, int offset){
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	c.add(Calendar.HOUR,offset);
        return c.getTime();
    }
    
    public static Date minOffset(Date date, int offset){
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	c.add(Calendar.MINUTE,offset);
        return c.getTime();
    }
    
    public static boolean isLastDayOfMonth(Date date){
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	int year = c.get(Calendar.YEAR);
    	int month = c.get(Calendar.MONTH);
    	int day = c.get(Calendar.DAY_OF_MONTH);
    	
    	c.set(year,month+1,1);
    	c.add(Calendar.DATE,-1);
    	int maxDay = c.get(Calendar.DAY_OF_MONTH);
    	
    	if(maxDay == day)
    		return true;
    	return false;
    }
    
    public static String calcHMS(Date beginday,Date endday ) {
        int hours, minutes, seconds;
        int timeInSeconds=ElapsedMillis(beginday,endday).intValue();
        String remintime=null;
        hours =(timeInSeconds / 3600);
        if(hours>24)
        	remintime=hours/24+1+"��";
        else
        {
        timeInSeconds = timeInSeconds - (hours * 3600);
        minutes = timeInSeconds / 60;
        timeInSeconds = timeInSeconds - (minutes * 60);
        seconds = timeInSeconds;
        remintime=hours+1 + "Сʱ";
        }
        return  remintime;
     }
    
    public static Long ElapsedMillis(Date beginday,Date endday)
    {
    	long l1 = beginday.getTime();
        long l2 = endday.getTime();
        long difference = l2 - l1;
        Long seconds = difference / 1000;
        return seconds;
    }

    /**
     * ����λ��
     * @return java.lang.String
     * @param str java.lang.String
     */
    private static String TimeFull(String str) {
        if (str.length() < 2)
          str = "0".concat(str);
        return str;
    }
    
    private static String TimeFull2(String str) {
        if (str.length() < 2)
          str = "00".concat(str);
        if(str.length()<3)
          str = "0".concat(str);
        return str;
    }
    
    /*
     * @得到月的�??1�??7后一�??1�??7.
     */
    
    //jia de ff................
    public static String LastDayOfMonth(int j){
    	int k=0;
		if(j==1) k=1;
		if(j==2) k=0;
		if(j==3) k=-1;
    	
    	Date date = new Date();
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	int year = c.get(Calendar.YEAR);
    	int month = c.get(Calendar.MONTH)+k;
    	int day = c.get(Calendar.DAY_OF_MONTH);
    	c.set(year,month,1);
    	c.add(Calendar.DATE,-1);
    	int maxDay = c.get(Calendar.DAY_OF_MONTH);
    	
    	return year+"-"+month+"-"+maxDay;
    }
    /*
     * @得到月的第一�??1�??7.
     */
    
    public static String FirstDayOfMonth(int j){
    	
    	int k=0;
		if(j==1) k=1;
		if(j==2) k=0;
		if(j==3) k=-1;
    	
    	Date date = new Date();
    	Calendar c = Calendar.getInstance();
    	c.setTime(date);
    	
    	int year = c.get(Calendar.YEAR );
    	int month = c.get(Calendar.MONTH )+k;
    	int day = c.get(Calendar.DAY_OF_MONTH );
    	
    	c.set(year,month,1);
    //	c.add(Calendar.DATE,1);
    	int minDay = c.get(Calendar.DAY_OF_MONTH);
    	
    	return year+"-"+month+"-"+minDay;
    }
    
	/**
	 * 生命周期
	 * 
	 * @param start
	 * @param end
	 * @return 0 D 0 H
	 */
	public static long Interval(Date start, Date end)
	{
		// 计算离最后登录相差时�??1�??7 几天 几小�??1�??7
		long days = 0;
		long hours = 0;
		
		if(start!=null && end!=null)			
		{
			long HOUR = 60L * 60L * 1000L;
			long allhours = (end.getTime() - start.getTime()) / HOUR;
			days = allhours / 24;
			hours = allhours - 24 * (days);
		}
		return days;
	}
	
	public static String getNowTime(){
		Calendar c = Calendar.getInstance();
		return DateUtil.formatDate3(c.getTime());
	}
	
	public static String getNowDate(){
		Calendar c = Calendar.getInstance();
		return DateUtil.formatshort(c.getTime());
	}
	
	public static String getLast10Str(){
		Calendar c = Calendar.getInstance();
		String s = DateUtil.formatDate4(c.getTime());
		return s.substring(4,s.length());
	}
	
	public static String getNowYear(){
		Calendar c = Calendar.getInstance();
		return DateUtil.formatYear(c.getTime());
	}
	
	public static String getMonth(String dateStr){
		return String.valueOf(Integer.parseInt(dateStr.split("-")[1]));
	}
	
	public static String getHour(String dateStr){
		return String.valueOf(Integer.parseInt(dateStr.substring(11,13)));
	}
	
	public static String getMinutes(String dateStr){
		return String.valueOf(Integer.parseInt(dateStr.substring(14,16)));
	}
	
	public static String getPreMonth26(String dateStr) throws ParseException{
		Calendar cl = Calendar.getInstance();
		cl.setTime(DateUtil.parse3(dateStr));
		cl.add(Calendar.MONTH, -1);
		return DateUtil.formatshort(cl.getTime()).split("-")[0]+"-"+DateUtil.formatshort(cl.getTime()).split("-")[1]+"-26";
	}
	
	public static String getThisMonth25(String dateStr) throws ParseException{

		return dateStr.split("-")[0]+"-"+dateStr.split("-")[1]+"-25";
	}
	
	public static String getMonthDayCount(String dateStr) throws ParseException{
		Calendar c = Calendar.getInstance();
    	c.setTime(DateUtil.parse2(dateStr));
    	return String.valueOf(c.getActualMaximum(Calendar.DAY_OF_MONTH));
	}
	
	public static String getAddDate(String date,int i,int type){
		Calendar cl = Calendar.getInstance();
		try{
			cl.setTime(DateUtil.parse2(date));
			if(type==0){
				cl.add(Calendar.DAY_OF_MONTH, +i);
			}else{
				cl.add(Calendar.DAY_OF_MONTH, -i);
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return DateUtil.formatDate3(cl.getTime());
	}
	
	public static String getAddHour(String date,int i,int type){
		Calendar cl = Calendar.getInstance();
		try{
			cl.setTime(DateUtil.parse3(date));
			if(type==0){
				cl.add(Calendar.HOUR_OF_DAY, +i);
			}else{
				cl.add(Calendar.HOUR_OF_DAY, -i);
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return DateUtil.formatDate3(cl.getTime());
	}
	
	public static String getAddMinutes(String date,int i,int type){
		Calendar cl = Calendar.getInstance();
		try{
			cl.setTime(DateUtil.parse3(date));
			if(type==0){
				cl.add(Calendar.MINUTE, +i);
			}else{
				cl.add(Calendar.MINUTE, -i);
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return DateUtil.formatDate3(cl.getTime());
	}
	
	public static String getAddSeconds(String date,int i,int type){
		Calendar cl = Calendar.getInstance();
		try{
			cl.setTime(DateUtil.parse3(date));
			if(type==0){
				cl.add(Calendar.SECOND, +i);
			}else{
				cl.add(Calendar.SECOND, -i);
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return DateUtil.formatDate3(cl.getTime());
	}
	
	public static String getDayWeek(String dateStr){
		Calendar cl = Calendar.getInstance();
		String[] weekDaysName = { "SUN", "MON", "TUR", "WED", "THU", "FRI", "SAT" };
		try{
			cl.setTime(DateUtil.parse2(dateStr));
		}catch(Exception e){
			e.printStackTrace();
		}
		return weekDaysName[cl.get(Calendar.DAY_OF_WEEK)-1];
	}
	
	public static String[] getAllYear(String startDate,String endDate){
		String[] year = null;
		try{
			String startYear = startDate.substring(0,4);
			String endYear = endDate.substring(0,4);
			int count = Integer.parseInt(endYear) - Integer.parseInt(startYear);
			year = new String[count+1];
			for(int i=0;i<=count;i++){
				year[i]=String.valueOf(Integer.parseInt(startYear)+i);
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return year;
	}
	
	public static String getYearByDate(String dateStr){
		String year = "";
		try{
			year = dateStr.split("-")[0];
		}catch(Exception e){
			e.printStackTrace();
		}
		return year;
	}
	
      
    public static void main(String[] args) throws ParseException{
    	Calendar cl = Calendar.getInstance();
		try{
			cl.setTime(DateUtil.parse3("2009-12-01 00:00:00"));
			cl.add(Calendar.MINUTE, +1);
			System.out.println(DateUtil.formatDate3(cl.getTime()));
		}catch(Exception e){
			e.printStackTrace();
		}
    }
}