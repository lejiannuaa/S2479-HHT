package com.hola.bs.bean;

import java.util.concurrent.ConcurrentHashMap;

import com.hola.bs.property.ConfigPropertyUtil;

/**
 * 用户对象
 * @author S1608
 *
 */
public class UserState {

	private static ConfigPropertyUtil c = new ConfigPropertyUtil();
	
	/**
	 * 用户名
	 */
	private String name;
	
	/**
	 * 用户所在门店
	 */
	private String store;
	
	/**
	 * 用户登录时间，该属性可用于查询是否超时
	 */
	private long loginTime;
	
	/**
	 * 用户特定文件夹的路径
	 */
	private String ownerFilePath;
	
	/**
	 * 登录用户的ip
	 */
	private String ip;
	
	/**
	 * 缓存
	 */
//	private HashMap<String, String> cache;
	private ConcurrentHashMap<String,String> cache;

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getStore() {
		return store;
	}

	public void setStore(String store) {
		this.store = store;
	}

	public long getLoginTime() {
		return loginTime;
	}

	public void setLoginTime(long loginTime) {
		this.loginTime = loginTime;
	}

	public String getIp() {
		return ip;
	}

	public void setIp(String ip) {
		this.ip = ip;
	}

	public String getOwnerFilePath() {
		return ownerFilePath;
	}

	public void setOwnerFilePath(String ownerFilePath) {
//		System.out.println(bundle);
//		System.out.println(bundle.containsKey("D1SHQT"));
//		System.out.println(bundle.containsKey("HHTPATH"));
//		System.out.println(bundle.containsKey("TIMEOUT"));
//		System.out.println("当前用户的数据交互目录为："+bundle.getString("HHTPATH")+"\\"+ownerFilePath);
		this.ownerFilePath = c.getValue("HHTPATH")+"\\"+ownerFilePath;
	}

	/**
	 * 获得用户缓存中的值
	 * @param name
	 * @return
	 */
	public String getAttribute(String name) {
		if(cache==null)
			return null;
		else
			return cache.get(name);
	}

	public synchronized void setAttribute(String name, String value) {
		if(cache == null)
			cache = new ConcurrentHashMap<String, String>();
		this.cache.put(name, value);
	}
	
	
}
