package com.hola.bs.bean;

import com.hola.bs.impl.Request;

public class BusinessBean {

	private Request request= null;
	private Response response= null;
	private UserState user=null;
	private String xmlname=null;
	
	
	public Response getResponse() {
		return response;
	}
	public void setResponse(Response response) {
		this.response = response;
	}
	public UserState getUser() {
		return user;
	}
	public void setUser(UserState user) {
		this.user = user;
	}
	public String getXmlname() {
		return xmlname;
	}
	public void setXmlname(String xmlname) {
		this.xmlname = xmlname;
	}
	public Request getRequest() {
		return request;
	}
	public void setRequest(Request request) {
		this.request = request;
	}
	
	
}
