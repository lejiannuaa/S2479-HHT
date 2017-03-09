package com.hola.common.util;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

public class TimeUtil 
{
	public static final String DATE_FORMAT_yyyyMMdd = "yyyyMMdd";
	public static final String DATE_FORMAT_hhmmss	= "HHmmss";
	public static final String DATE_FORMAT_yyyyMMddHHmmssSSS	= "yyyyMMddHHmmssSSS";
	public static final String DATE_FORMAT_yyyyMMddHHmmss	= "yyyyMMddHHmmss";
	
	public static void main(String[] args) {
		System.out.println(TimeUtil.formatDate(new Date(), TimeUtil.DATE_FORMAT_hhmmss));
	}
	
	
	public static Date parseDayForStr(String day)
	{
		return parseDay(day , "yyyyMMddHHmmss");
	}
	public static String formatDateToString(Date date)
	{
		return formatDate(date , "yyyyMMddHHmmssSSS");
		
	}
	public static Date parseDay(String day , String format)
	{
		if(day == null)
			return null;
		Date date = null;
		SimpleDateFormat df = new SimpleDateFormat(format);
		try
		{
			date = df.parse(day);
		}catch (ParseException e) 
		{ 
			e.printStackTrace();
		}
		return date;
	}
	
	public static String formatDate(Date date , String format)
	{
		if (date == null)
		{
			return "";
		} else
		{
			SimpleDateFormat df = new SimpleDateFormat(format);
			return df.format(date);
		}
	}
}
