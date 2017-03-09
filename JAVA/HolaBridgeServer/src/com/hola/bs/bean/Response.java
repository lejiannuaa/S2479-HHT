package com.hola.bs.bean;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;

import com.hola.bs.service.BusinessService;

public class Response {
	private String response;
	private String code = BusinessService.successcode;
	private String desc;
	private String store;
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

	public Response() {
	}

	/**
	 * 
	 * @param response
	 * @param code
	 * @param desc
	 */
	public Response(String response, String code, String desc) {
		this.response = response;
		this.code = code;
		this.desc = desc;
	}

	public String getResponse() {
		return response;
	}

	public void setResponse(String response) {
		this.response = response;
	}

	public String getCode() {
		return code;
	}

	public void setCode(String code) {
		this.code = code;
	}

	public String getDesc() {
		return desc;
	}

	public void setDesc(String desc) {
		this.desc = desc;
	}

	public String toString() {
		return "response=" + response + ";code=" + code + ";desc=" + desc
				+ ";date=" + date;
	}

	public String getStore() {
		return store;
	}

	public void setStore(String store) {
		this.store = store;
	}
}
