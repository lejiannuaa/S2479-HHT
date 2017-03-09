package com.hola.bs.core.schedule;

import java.util.Timer;

import com.hola.bs.property.ConfigPropertyUtil;

/**
 * 传MQ的定时器
 * @author S2139
 * 2012 Nov 2, 2012 2:26:15 PM 
 */
public class SendMqTimer {

	private ConfigPropertyUtil c = new ConfigPropertyUtil();
	public SendMqTimer(){
		
		Timer timer = new Timer();
		UploadToMqTask u = new UploadToMqTask();
		timer.schedule(u, 30000, (c.getIntValue("periodMillSeconds")));
		
	}
	
}
