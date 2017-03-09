package com.task.common;

import java.math.BigDecimal;

public class MathUtil {

	public static void main(String[] args) {

		  double a = 200;
		  double b = 1;
		  double c = divide(b, a, 1);
		  System.out.println(addZero(c) + "%");
		 }

	 public static double divide(double v1, double v2, int scale) {
		  double c = div(v1, v2, scale + 2);
		  // System.out.println(c+"%");
		  return div(c * 100.0, 1, scale);
	 }
	 
	 /**
	  * 提供（相?）精?的除法?算。当?生除不尽的情况?，由scale参数指 定精度，以后的数字四舍五入。
	  * 
	  * @param v1
	  *            被除数
	  * @param v2
	  *            除数
	  * @param scale
	  *            表示表示需要精?到小数点以后几位。
	  * @return ?个参数的商
	  */
	 public static double div(double v1, double v2, int scale) {
	  if (scale < 0) {
	   throw new IllegalArgumentException(
	     "The scale must be a positive integer or zero");
	  }
	  BigDecimal b1 = new BigDecimal(Double.toString(v1));
	  BigDecimal b2 = new BigDecimal(Double.toString(v2));
	  return b1.divide(b2, scale, BigDecimal.ROUND_HALF_UP).doubleValue();
	 }

	 /**
	  * 提供精?的小数位四舍五入?理。
	  * 
	  * @param v
	  *            需要四舍五入的数字
	  * @param scale
	  *            小数点后保留几位
	  * @return 四舍五入后的?果
	  */
	 public static double round(double v, int scale) {
	  if (scale < 0) {
	   throw new IllegalArgumentException(
	     "The scale must be a positive integer or zero");
	  }
	  BigDecimal b = new BigDecimal(Double.toString(v));
	  BigDecimal one = new BigDecimal("1");
	  return b.divide(one, scale, BigDecimal.ROUND_HALF_UP).doubleValue();
	 }

	 public static String addZero(double v) {
	  StringBuffer s = null;
	  String temp = String.valueOf(v);
	  //String[] tempArray = temp.split(".");
	  int point = temp.indexOf(".");
	  int length = temp.length();
	  int n = length - 1 - point;
	  
	  if (n > 0) {
	   s = new StringBuffer(temp);
	   for ( int i = 0; i < n; i++) {
	    s.append(0);
	   }
	   return s.toString();
	  } else {
	   return temp;
	  }

	 }
}
