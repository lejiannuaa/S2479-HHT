package com.task.common;

import java.text.ParseException;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

public class CalendarUtil {

	/**
	 * 取得指定日期的所处星期的第一�?
	 * @param date 指定日期�?
	 * @return 指定日期的所处星期的第一�?
	 */
	public static java.util.Date getFirstDayOfWeek(java.util.Date date) {
		/**
		 * 详细设计�? 
		 * 1.如果date是星期日，则�?0�? 
		 * 2.如果date是星期一，则�?1�? 
		 * 3.如果date是星期二，则�?2�?
		 * 4.如果date是星期三，则�?3�? 
		 * 5.如果date是星期四，则�?4�? 
		 * 6.如果date是星期五，则�?5�?
		 * 7.如果date是星期六，则�?6�?
		 */
		GregorianCalendar gc = (GregorianCalendar) Calendar.getInstance();
		gc.setTime(date);
		switch (gc.get(Calendar.DAY_OF_WEEK)) {
		case (Calendar.SUNDAY  ):
			gc.add(Calendar.DATE, 0);
			break;
		case (Calendar.MONDAY  ):
			gc.add(Calendar.DATE, -1);
			break;
		case (Calendar.TUESDAY  ):
			gc.add(Calendar.DATE, -2);
			break;
		case (Calendar.WEDNESDAY  ):
			gc.add(Calendar.DATE, -3);
			break;
		case (Calendar.THURSDAY  ):
			gc.add(Calendar.DATE, -4);
			break;
		case (Calendar.FRIDAY  ):
			gc.add(Calendar.DATE, -5);
			break;
		case (Calendar.SATURDAY  ):
			gc.add(Calendar.DATE, -6);
			break;
		}
		return gc.getTime();
	}

	/**
	 * 取得指定日期的所处星期的�?后一�?
	 * 
	 * @param date
	 *            指定日期�?
	 * @return 指定日期的所处星期的�?后一�?
	 */
	public static java.util.Date getLastDayOfWeek(java.util.Date date) {
		/**
		 * 详细设计�? 
		 * 1.如果date是星期日，则�?6�? 
		 * 2.如果date是星期一，则�?5�? 
		 * 3.如果date是星期二，则�?4�?
		 * 4.如果date是星期三，则�?3�? 
		 * 5.如果date是星期四，则�?2�? 
		 * 6.如果date是星期五，则�?1�?
		 * 7.如果date是星期六，则�?0�?
		 */
		GregorianCalendar gc = (GregorianCalendar) Calendar.getInstance();
		gc.setTime(date);
		switch (gc.get(Calendar.DAY_OF_WEEK)) {
		case (Calendar.SUNDAY  ):
			gc.add(Calendar.DATE, 6);
			break;
		case (Calendar.MONDAY  ):
			gc.add(Calendar.DATE, 5);
			break;
		case (Calendar.TUESDAY  ):
			gc.add(Calendar.DATE, 4);
			break;
		case (Calendar.WEDNESDAY  ):
			gc.add(Calendar.DATE, 3);
			break;
		case (Calendar.THURSDAY  ):
			gc.add(Calendar.DATE, 2);
			break;
		case (Calendar.FRIDAY  ):
			gc.add(Calendar.DATE, 1);
			break;
		case (Calendar.SATURDAY  ):
			gc.add(Calendar.DATE, 0);
			break;
		}
		return gc.getTime();
	}
	
	public static int compareDate(String date1,String date2){
        java.util.Calendar c1=java.util.Calendar.getInstance();
        java.util.Calendar c2=java.util.Calendar.getInstance();
        try{
            c1.setTime(DateUtil.parse2(date1));
            c2.setTime(DateUtil.parse2(date2));
        }catch(java.text.ParseException e){
            System.err.println("格式不正�?");
        }
        int result=c1.compareTo(c2);
        if(result==0){
//        	 System.out.println("相等");
        	 return 0;
        }else if(result<0){
//        	 System.out.println("小于");
        	 return 1;
        }else{
//            System.out.println("大于");
            return 2;
        }
	}
	
	public static int compareDateTime(String dateStr1,String dateStr2){
		java.util.Calendar c1=java.util.Calendar.getInstance();
        java.util.Calendar c2=java.util.Calendar.getInstance();
//        System.out.println("dateStr1 = "+dateStr1+"   dateStr2 = "+dateStr2);
        try{
            c1.setTime(DateUtil.parse3(dateStr1));
            c2.setTime(DateUtil.parse3(dateStr2));
        }catch(java.text.ParseException e){
            System.err.println("格式不正�?");
        }
        int result=c1.compareTo(c2);
        if(result==0){
//        	 System.out.println("相等");
        	 return 0;
        }else if(result<0){
//        	 System.out.println("小于");
        	 return 1;
        }else{
//            System.out.println("大于");
            return 2;
        }
	}
	
	public static long getCompMinutes(String dateStr1,String dateStr2) throws ParseException{
		Calendar date1 = Calendar.getInstance();
		date1.setTime(DateUtil.parse3(dateStr1));
		Calendar date2 = Calendar.getInstance();
		date2.setTime(DateUtil.parse3(dateStr2));
		Long compVal = date1.getTime().getTime() - date2.getTime().getTime();
		return compVal/1000/60;
	}
	
	public static long getCompHour(String dateStr1,String dateStr2) throws ParseException{
		Calendar date1 = Calendar.getInstance();
		date1.setTime(DateUtil.parse3(dateStr1));
		Calendar date2 = Calendar.getInstance();
		date2.setTime(DateUtil.parse3(dateStr2));

		Long compVal = date1.getTime().getTime() - date2.getTime().getTime();
		
		return compVal/1000/60/60;
	}
	
	public static long getCompDate(String dateStr1,String dateStr2) throws ParseException{
		Calendar date1 = Calendar.getInstance();
		date1.setTime(DateUtil.parse2(dateStr1));
		Calendar date2 = Calendar.getInstance();
		date2.setTime(DateUtil.parse2(dateStr2));
		Long compVal = date1.getTime().getTime() - date2.getTime().getTime();
		return compVal/1000/60/60/24;
	}
	
	public static String[] getAllDate(String dateStr1,String dateStr2){
		String[] date = null;
		try{
			long dateCount = getCompDate(dateStr1,dateStr2);
			date = new String[(int) (dateCount+1)];
			for(int i=0;i<=dateCount;i++){
				date[i] = DateUtil.getAddDate(dateStr2, i, 0);
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return date;
	}
	
	/**
	 * 获得双休�?
	 * @param dataAry
	 * @return
	 */
	public static String[] getHoliday(String[] dateAry){
		String[] date = null;
		try{
			for(int i=0;i<dateAry.length;i++){
				if(DateUtil.getDayWeek(dateAry[i]).equals("SAT")||DateUtil.getDayWeek(dateAry[i]).equals("SUN")){
					if(date==null){
						date = new String[1];
						date[0] =  dateAry[i];
					}else{
						String[] tempdate = date;
						date = new String[date.length+1];
						for(int j=0;j<tempdate.length;j++){
							date[j] = tempdate[j];
						}
						date[date.length-1] = dateAry[i];
					}
				}
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return date;
	}
	
	public static void main(String[] args) throws ParseException{
		
//		System.out.println(DateUtil.formatshort(CalendarUtil.getLastDayOfWeek(DateUtil.parse2("2009-04-02"))));
//		System.out.println(DateUtil.formatshort(CalendarUtil.getFirstDayOfWeek(DateUtil.parse2("2009-04-03"))));
//		
//		Calendar cl1 = Calendar.getInstance();
//		cl1.setTime(DateUtil.parse2("2009-04-01"));
//		
//		Calendar cl2 = Calendar.getInstance();
//		cl2.setTime(DateUtil.parse2("2009-04-06"));
//		
//		long a = cl2.getTime().getTime();
//		long b = cl1.getTime().getTime();
//		System.out.print((a-b)/(24*3600*1000));
//		CalendarUtil.compareDate("2009-04-04", "2009-04-06");
//		System.out.println(CalendarUtil.getCompDate("2009-04-13", "2009-04-12"));
//		String[] d = CalendarUtil.getAllDate("2009-04-20", "2009-04-12");
//		for(int i=0;i<d.length;i++){
//			System.out.println(d[i]);
//		}
//		System.out.println(CalendarUtil.compareDateTime(DateUtil.getNowTime(),"2009-06-10 1:00:00"));
//		System.out.println(DateUtil.parse2("2009-05-06 09:00:00"));
		
//        if(result==0){
////       	 System.out.println("相等");
//       	 return 0;
//       }else if(result<0){
////       	 System.out.println("小于");
//       	 return 1;
//       }else{
////           System.out.println("大于");
//           return 2;
//       }
//		CalendarUtil.getAllDate("2009-01-04", "2008-12-25");
//		System.out.println(50%60);
//		System.out.println(CalendarUtil.getCompDate("2009-06-15","2009-06-10"));
		System.out.println(CalendarUtil.getCompMinutes("2009-06-15 09:45:00", "2009-06-15 08:40:00"));
	}

}
