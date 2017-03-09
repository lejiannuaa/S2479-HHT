package com.hola.bs.util;

import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Map;

/**
 * 通用工具类，该类的方法都为静态方法
 * @author S1608
 * 
 */
public class CommonStaticUtil {
	
//	private static ApplicationContext ctx=new ClassPathXmlApplicationContext("applicationContext.xml");
//	
//	/**
//	 * 返回Spring ClassPathXmlApplicationContext对象的引用
//	 * @return
//	 */
//	public static ApplicationContext getApplicationContext(){
//		return ctx;
//	}
//	
//	public static synchronized Object getBean(String name){
//		return ctx.getBean(name);
//	}


	public static Long numIsNull(Object value){
		if(value==null)
			return null;
		else 
			return Long.valueOf(value.toString());
	}
	public static String strIsNull(Object value){
		if(value==null)
			return null;
		else 
			return value.toString().trim();
	}
	
	/**
	 * 获得特定格式的时间字符串
	 * @return 格式：20110910
	 */
	public static String getDate() {
		Calendar c = Calendar.getInstance();
		c.setTime(new Date());
		String year = String.valueOf(c.get(Calendar.YEAR));
		String month = String.valueOf(c.get(Calendar.MONTH) + 1);
		if (month.length() == 1)
			month = "0" + month;
		String date = String.valueOf(c.get(Calendar.DATE));
		if (date.length() == 1)
			date = "0" + date;
		return year + month + date;
//		return "20110331";
	}
	
	/**
	 * 根据传入的字符串格式产生日期
	 * @param parten: YYYYMMDDHHMMSS
	 * @return
	 */
	public static  String dataFormat(String parten) {
		parten = parten.toLowerCase();

		// 获得mm字符串所在的下标，用以判断该mm字符串代表的意义。
		int start = parten.indexOf("mm");

		// MM如果前面如果是HH，则认为是分钟的含义，否则表示月份的含义
		if (start > 0) {
			char a = parten.charAt(start - 1);
			if (a == 'h')
				parten = parten.replace("hh", "HH");
			else
				parten = parten.replaceFirst("mm", "MM").replace("hh", "HH");
		}

//Out.println(parten);
		// 获得格式化后的值
		String value = new SimpleDateFormat(parten).format(new Date());
		return value;
	}
	
	/**
	 * 将List中指定的列以数组的方式呈现
	 * @param list
	 * @param key
	 * @return
	 * author: S2139
	 * 2012 Nov 2, 2012 2:03:17 PM
	 */
	public static String[] tranListMapToArray(List<Map> list, String key){
		String[] array = null;
		if(list!=null&&list.size()>0){
			array = new String[list.size()];
			int no = 0;
			for(Map m : list){
				array[no] = m.get(key).toString();
				no++;
			}
		}
		
		return array;
	}
	
	public static Double doubleMath(Double allPrice) {
		DecimalFormat df = new DecimalFormat("##.00");
		String format = df.format(allPrice);
		return Double.valueOf(format);
	}
}

