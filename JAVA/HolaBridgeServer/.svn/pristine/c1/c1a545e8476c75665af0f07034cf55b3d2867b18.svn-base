package com.hola.bs.core;

import java.util.TimerTask;

import org.springframework.beans.factory.annotation.Autowired;


/**
 * 定时任务，定时查询是否有用户已经超时
 * @author S1608
 *
 */
public class TimeoutCheck extends TimerTask{

	@Autowired(required=true)
	private UserContainer userContainer;
	
	/**
	 * 用户允许登录的最大时间,从用户登录的时间开始计算，如果超过timeout允许的最大时间，则被认定为超时
	 */
	private long timeout;
	
	/**
	 * 执行间隔时间
	 */
	private long interval;
	
	@Override
	public void run() {
		// TODO Auto-generated method stub
		
	}
	
	/**
	 * 用户是否已经超时
	 * @param id
	 * @return
	 */
	private boolean isTimeout(String id){
		return false;
	}

}
