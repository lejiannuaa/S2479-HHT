package com.task.trigger;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.List;

import org.apache.log4j.Logger;
import org.quartz.CronTrigger;
import org.quartz.JobDetail;
import org.quartz.Scheduler;
import org.quartz.SchedulerException;
import org.quartz.SchedulerFactory;
import org.quartz.impl.StdSchedulerFactory;

import com.task.bean.FileExchange;
import com.task.bean.SAPBean;
import com.task.common.DateUtil;
import com.task.common.PropertysUtil;


/**
 * ��ʼ���ƻ�����
 * @author sky
 *
 */
public class Trigger{

	private static SchedulerFactory sf ;
	
	private static Scheduler sched ;
	
	private static PropertysUtil pu = new PropertysUtil();
	
	private static Logger logger = Logger.getLogger( Trigger.class );
	
	
	public Trigger() throws SchedulerException{
		this.sf = new StdSchedulerFactory();
		this.sched = sf.getScheduler();
	}
	
	public void run() throws Exception {
      
        System.out.println("***** Scheduling Jobs Starting *****");
        
        Hashtable startParamHT = pu.getPlanStartTime();//�ƻ�����������Ϣ
//        List connectInfoList = pu.getConnectInfo();//���ֵ����ݿ�������Ϣ
//        for(int i=0;i<connectInfoList.size();i++){
//        	Hashtable connectInfoHT = (Hashtable) connectInfoList.get(i);
        	JobDetail job = new JobDetail("job", "groupExchange", FileExchange.class);
//        	job.getJobDataMap().put("connectInfo",connectInfoHT);
        	job.setRequestsRecovery(true);//0 0/"+startParamHT.get("PollingCycle")+" * * * ?"
        	CronTrigger trigger = new CronTrigger("trigger", "groupExchange", "job", "groupExchange", "0 0/"+startParamHT.get("PollingCycle").toString()+" * * * ?");//����job��ѵ�����Լ�������Ϣ
        	trigger.setStartTime(DateUtil.parse3(startParamHT.get("StartTime").toString()));//����job��ʼʱ��
        	sched.addJob(job, true);
        	sched.scheduleJob(trigger);
//        }
		 sched.start();
		 System.out.println("Jobs start time��"+startParamHT.get("StartTime").toString()+"  pollingCycle��"+startParamHT.get("PollingCycle").toString()+" mins");
		 System.out.println("***** Scheduling Jobs Started *****");
	}
	
	public static void main(String[] args){
		
		try {
			PropertysUtil pu = new PropertysUtil();
			Trigger trigger = new Trigger();
			
			pu.initPropertysUtil();
			pu.getDBConnInfo();
			
			/*
			SAPBean sapBean = new SAPBean();
			sapBean.run();
			
			FileExchange file = new FileExchange();
			file.run();
			*/
			trigger.run();
			
		} catch (Exception e) {
			e.printStackTrace();
		}
		
	}
}
