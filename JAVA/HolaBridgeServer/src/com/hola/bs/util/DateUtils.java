package com.hola.bs.util;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

import org.apache.commons.lang.StringUtils;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

/**
 * 日期操作辅助类
 * 
 * @author s1788
 * 
 */
public class DateUtils {

	public static final int ID_DEFAULT = 0;
	public static final int ID_YYYYMMDD = 1;

	private static final String YYYY_MM_DD = "yyyy-MM-dd";
	private static final String YYYYMMDD = "yyyyMMdd";
	private static final String YYYY_MM_DD_HH_MI_SS_MS = "yyyy-MM-dd HH:mm:ss:SSS";
	public static final String DEFAULT_PAGE_FORMAT = "yyyyMMddHHmm";

	private DateUtils() {
	}

    public static Date get(String date) {
        String year = date.substring(0, 4);
        String month = date.substring(4, 6);
        String day = date.substring(6, 8);
        String hour = date.substring(8, 10);
        String minute = date.substring(10, 12);
        String second = date.substring(12, 14);
        String millisecond = date.substring(14, 17);
        Calendar c = Calendar.getInstance();
        c.set(Integer.parseInt(year), Integer.parseInt(month) - 1, Integer
                        .parseInt(day), Integer.parseInt(hour), Integer
                        .parseInt(minute), Integer.parseInt(second));
        c.set(Calendar.MILLISECOND, Integer.parseInt(millisecond));
        return c.getTime();
}
    public static Date getTime(String date) {
        String year = date.substring(0, 4);
        String month = date.substring(4, 6);
        String day = date.substring(6, 8);
        String hour = date.substring(8, 10);
        String minute = date.substring(10, 12);
        String second = date.substring(12, 14);
  
        Calendar c = Calendar.getInstance();
        c.set(Integer.parseInt(year), Integer.parseInt(month) - 1, Integer
                        .parseInt(day), Integer.parseInt(hour), Integer
                        .parseInt(minute), Integer.parseInt(second));

        return c.getTime();
}
    
	public static String get(Date date) {
		if (date == null) {
			return "00000000000000000";
		}
		String result = "";
		Calendar c = Calendar.getInstance();
		c.setTime(date);
		int year = c.get(Calendar.YEAR);
		int month = c.get(Calendar.MONTH) + 1;
		int day = c.get(Calendar.DAY_OF_MONTH);
		int hour = c.get(Calendar.HOUR_OF_DAY);
		int minute = c.get(Calendar.MINUTE);
		int second = c.get(Calendar.SECOND);
		int millisecond = c.get(Calendar.MILLISECOND);
		result += year;
		if (month < 10) {
			result += "0";
		}
		result += month;
		if (day < 10) {
			result += "0";
		}
		result += day;
		if (hour < 10) {
			result += "0";
		}
		result += hour;
		if (minute < 10) {
			result += "0";
		}
		result += minute;
		if (second < 10) {
			result += "0";
		}
		result += second;
		if (millisecond < 10) {
			result += "00";
		} else if (millisecond < 100) {
			result += "0";
		}
		result += millisecond;
		return result;
	}
	/**
	 * 获取今年年份
	 * 
	 * @return int 今年年份
	 */
	public static int getNowYear() {
		// 获取年份
		return Calendar.getInstance().get(Calendar.YEAR);
	}

	public static String format(Date date, String format) {
		if (date == null) {
			date = get("00000000000000000");
		}
		return format(get(date), format);
	}

	

	public static String format(String date, String format) {
		return date.replaceAll(
				"(\\d{4})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{3})",
				format.replaceAll("yyyy", "\\$1").replaceAll("MM", "\\$2")
						.replaceAll("dd", "\\$3").replaceAll("HH", "\\$4")
						.replaceAll("mm", "\\$5").replaceAll("ss", "\\$6")
						.replaceAll("SSS", "\\$7"));
	}

	/**
	 * 取得系统时间
	 * 
	 * @return Date 系统时间
	 */
	public static Date getSystemDate() {
		// 取得系统时间
		return Calendar.getInstance().getTime();
	}

	/**
	 * String转换Date,如果转换字符串对象为null或"",则返回null。
	 * 
	 * @param source
	 *            转换字符串对象
	 * @param format
	 *            转换格式
	 * @return 转换后的日期
	 * 
	 * @throws ParseException
	 *             转换失败异常
	 */
	public static java.util.Date string2Date(String source, String pattern) {
		if (StringUtils.isEmpty(source)) {
			return null;
		}
		try {
			return new SimpleDateFormat(pattern).parse(source);
		} catch (ParseException e) {
			throw new RuntimeException("字符串" + source + "转换日期格式为" + pattern
					+ "的处理失败", e);
		}

	}

	/**
	 * String转换日期格式为yyyy-MM-dd的Date
	 * 
	 * @param source
	 *            转换字符串
	 * @return 转换后的日期
	 * 
	 * @throws ParseException
	 *             转换失败异常
	 */
	public static java.util.Date string2Date(String source) {
		return string2Date(source, YYYY_MM_DD);
	}

	/**
	 * 通用的日期字符串转日期方法，今后将根据实际情况进行拓展
	 * 
	 * @param source
	 *            日期字符串参数
	 * @param i
	 *            需要转换的类型标识
	 * @return date类型日期对象 author: S2139 2012 Sep 4, 2012 10:26:50 AM
	 */
	public static java.util.Date string2Date(String source, int i) {
		Date date = null;
		switch (i) {
		case 1:
			date = string2Date(source, YYYYMMDD);
			break;
		default:
			date = string2Date(source);
			break;
		}
		return date;
	}

	/**
	 * 获取完整时间，到毫秒数
	 * 
	 * @param date
	 * @return author: S2139 2012 Nov 6, 2012 7:16:50 PM
	 */
	public static String string2TotalTime(Date date) {
		if (date == null) {
			return "";
		}
		return DateFormatToString(date, YYYY_MM_DD_HH_MI_SS_MS);
	}

	/**
	 * 日期格式为yyyy-MM-dd的Date转换String
	 * 
	 * @param date
	 *            日期
	 * @return 转换后的字符串
	 */
	public static String date2String(Date date) {
		if (date == null) {
			return "";
		}
		return DateFormatToString(date, YYYY_MM_DD);
	}

	public static String date2StringDate(Date date) {
		if (date == null) {
			return "";
		}
		return DateFormatToString(date, YYYYMMDD);
	}

	/**
	 * 日期格式为YYYYMMDD的date转换String
	 * 
	 * @param date
	 * @return 转换后的字符串 author: S2139 2012 Aug 22, 2012 4:25:56 PM
	 */
	public static String date2String2(Date date) {
		if (date == null) {
			return "";
		}
		return DateFormatToString(date, YYYYMMDD);
	}

	public static String DateFormatToString(Date date, String formatString) {
		SimpleDateFormat frm1 = new SimpleDateFormat(formatString);
		return frm1.format(date);
	}

	/**
	 * "yyyyMMdd"日期 转换为 "yyyy年MM月dd日"
	 * 
	 * @param date
	 * @param format
	 * @return
	 */
	public static String str2ChineseDate(String date) {
		return DateUtils.DateFormatToString(
				DateUtils.string2Date(date, "yyyyMMdd"), "yyyy年MM月dd日");
	}

}
