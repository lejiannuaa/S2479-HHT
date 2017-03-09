package com.task.common;

import org.apache.commons.lang.StringUtils;

public class StringUtil {

	public static String rightPad(String str){
		try{
			if (str.length() >= 1000 && str.length() <= 2000) {
				str = StringUtils.rightPad(str, 2001,"*");
	        }
		}catch(Exception e){
			e.printStackTrace();
		}
		return str;
	}
	
	public static void main(String[] args){
		System.out.println(StringUtil.rightPad("a").length());
	}
}
