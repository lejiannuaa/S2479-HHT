package com.hola.bs.core.schedule;

import java.io.IOException;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import com.hola.bs.core.mail.MailUtils;
import com.hola.bs.util.SpringUtil;
import com.iss.hdc.Converter;
import com.iss.hdc.base.exception.HolaDataConversionException;

/**
 * 这个线程是发送信息到MQ用的
 * @author S2139
 * 2012 Nov 2, 2012 11:38:52 AM 
 */
public class SendMqThread extends Thread {
	
	private Log log = LogFactory.getLog(getClass());
	private String store;
	private String[] instnos;
	
	public SendMqThread() {
		super();
		// TODO Auto-generated constructor stub
	}

	public SendMqThread(String store, String[] instnos) {
		super();
		this.store = store;
		this.instnos = instnos;
	}

	public String getStore() {
		return store;
	}

	public void setStore(String store) {
		this.store = store;
	}

	public String[] getInstnos() {
		return instnos;
	}

	public void setInstnos(String[] instnos) {
		this.instnos = instnos;
	}
	
	public void run(){
		for(String instno:instnos){
			this.updateOnMq(this.getStore(),instno);
		}
	}
	
	/**
	 * 上传数据到MQ
	 * @param hhtschema
	 * @param instno
	 * @return
	 * author: S2139
	 * 2012 Nov 2, 2012 1:31:50 PM
	 */
	private String updateOnMq(String store, String instno){
		Converter converter = null;
		Object o = SpringUtil.getBean("hdcConverter");
		if(o == null){
			ApplicationContext ctx = new ClassPathXmlApplicationContext("spring.xml");
			converter = (Converter)ctx.getBean("hdcConverter");
		}else{
			converter = (Converter)o;
		}
		try {
			converter.createJsonAndUploadMq("hht"+store, instno);
		} catch (HolaDataConversionException e) {
			// TODO Auto-generated catch block
			sendExceptionMail(store,instno);
			log.error("mq transaction error!", e);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			sendExceptionMail(store,instno);
			log.error("mq transaction error!", e);
		} catch(Exception e){
			log.error("launch mq funtion error!",e);
		}
		finally{
			return "";
		}
	}
	
	/**
	 * MQ处理失败时，发邮件
	 * author: S2139
	 * 2012 Nov 2, 2012 1:32:06 PM
	 */
	private void sendExceptionMail(String store,String instno){
		
		MailUtils.sendMail("HHT作业系统错误，店号："+store+"，处理批次号："+instno, 
				"门店"+store+"在处理批次号为"+instno+"的作业时，产生错误，请管理员手动协助处理。");
	}
}
