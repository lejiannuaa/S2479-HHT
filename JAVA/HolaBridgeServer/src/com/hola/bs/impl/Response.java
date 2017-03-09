package com.hola.bs.impl;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;


public class Response {
    public static String SUCCESS = "0";
    public static String FAIL = "1";
    private String response = "";
    private String code = "0";
    private String desc = "";
    /**
	 * 规则20130315 同步操作时间 
	 * @since 2013-03-14
	 * by S2138
	 */ 
	private String date = getDateValue();

	private static String getDateValue(){
		Calendar calendar = Calendar.getInstance();
		DateFormat df = new SimpleDateFormat("yyyyMMdd");
		return df.format(calendar.getTime());
	}

    public String getDate() {
		return date;
	}

	public void setDate(String date) {
		this.date = date;
	}

	public Response(){
    }

    public String getResponse() {
        return response;
    }

    public String getCode() {
        return code;
    }

    public String getDesc() {
        return desc;
    }

    public void setResponse(String response) {
        this.response = response;
    }

    public void setCode(String code) {
        this.code = code;
    }

    public void setDesc(String desc) {
        this.desc = desc;
    }

    @Override
    public String toString() {
        return "response=" + response + ";code=" + code + ";desc=" + desc+ ";date=" + date;
               
    }
}
