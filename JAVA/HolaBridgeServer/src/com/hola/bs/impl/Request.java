package com.hola.bs.impl;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class Request {
	public static String REQUEST = "request";
	public static String USR = "usr";
	private ConcurrentHashMap<String, String> parameterMap = null;

	public Request(String request) {
		parseRequset(request);
	}

	private void parseRequset(String request) {

		if (request == null || request.trim().equals(""))
			return;

		parameterMap = new ConcurrentHashMap<String, String>();
		
		for (String s : request.split(";")) {
			String[] paramArray = s.split("=");
//			System.out.println("===="+paramArray[0]);
			parameterMap.put(paramArray[0], (paramArray.length==1)?"":paramArray[1]);
		}
	}
	
	/**
	 * 获得程序ID，request=001;op=01，即可得到程序ID为001_01。
	 * @return 程序ID代号
	 */
	public String getRequestID(){
		String request = parameterMap.get(REQUEST);
		String op = parameterMap.get("op");
		if(op==null)
			return request;
		else
			return request+"_"+op;
	}

	/**
	 * 获得对应的值
	 * @param key 值的名称
	 * @return
	 */
	public String getParameter(String key) {
		return parameterMap.get(key);
	}
	
	public static void main(String[] args){
		String a = "request=login;usr=S1777;pwd=123456;times=1;sn=03FF2DDDFAA";
		String b = "request=101;usr=S1777;op=02;bc=000889";
		Request r = new Request(b);
		System.out.println(r.getRequestID());
	}

	public Map<String, String> getParameterMap() {
		return parameterMap;
	}
}
