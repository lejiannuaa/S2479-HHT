package com.hola.bs.json;

/**
 * JSON里面 config属性的一些参数信息"config":{"type":"1","direction":".....","id":"00342"}
 * @author S2139
 * 2012 Aug 24, 2012 10:32:11 AM 
 */
public class Config {
	
	private String type;
	private String direction;
	private String id;
	
	
	public Config() {
		// TODO Auto-generated constructor stub
	}
	
	public Config(String type, String direction, String id) {
		this.type = type;
		this.direction = direction;
		this.id = id;
	}
	public String getType() {
		return type;
	}
	public void setType(String type) {
		this.type = type;
	}
	public String getDirection() {
		return direction;
	}
	public void setDirection(String direction) {
		this.direction = direction;
	}
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}
	
	
}
