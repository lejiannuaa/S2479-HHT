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

import com.task.bean.ControlBean;
import com.task.bean.invokeBean;
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
	
	private static ControlBean control = new ControlBean();
	
	private static Logger logger = Logger.getLogger( Trigger.class );
	
	
	public Trigger() throws SchedulerException{
		this.sf = new StdSchedulerFactory();
		this.sched = sf.getScheduler();
	}
	
	public void run() throws Exception {
      
        System.out.println("***** Scheduling Jobs Starting *****");
        Hashtable ht = control.getInvokeList();
        Iterator it = ht.keySet().iterator();
        Hashtable startParamHT = pu.getPlanStartTime();//�ƻ�����������Ϣ
        while(it.hasNext()){
        	String CHGCODE = it.next().toString();
        	Hashtable infoHT = (Hashtable) ht.get(CHGCODE);
        	JobDetail job = new JobDetail("job"+CHGCODE, "groupControl", invokeBean.class);
        	job.getJobDataMap().put("CHGSTART",infoHT.get("CHGSTART"));//������ʼʱ��
        	job.getJobDataMap().put("CHGEND",infoHT.get("CHGEND"));//������ֹʱ��
        	job.getJobDataMap().put("CHGFRQ",infoHT.get("CHGFRQ"));//����Ƶ��	D:�� H:Сʱ M:���� S:��
        	job.getJobDataMap().put("CHGVALUE",infoHT.get("CHGVALUE"));//����Ƶ��ֵ
        	job.getJobDataMap().put("COMMAND",infoHT.get("COMMAND"));//��������
        	job.getJobDataMap().put("CHGCODE",CHGCODE);//��������
        	
        	job.setRequestsRecovery(true);
        	String pollVal = "";
        	if(infoHT.get("CHGFRQ").toString().equals("D")){//��
        		pollVal = "0 0 0 1/"+infoHT.get("CHGVALUE").toString()+" * ?";
        	}else if(infoHT.get("CHGFRQ").toString().equals("H")){//Сʱ
        		pollVal = "0 0 0/"+infoHT.get("CHGVALUE").toString()+" * * ? ";
        	}else if(infoHT.get("CHGFRQ").toString().equals("M")){//����
        		pollVal = "0 0/"+infoHT.get("CHGVALUE").toString()+" * * * ?";
        	}else if(infoHT.get("CHGFRQ").toString().equals("S")){//��
        		pollVal = "0/"+infoHT.get("CHGVALUE").toString()+" * * * * ?";
        	}
        	CronTrigger trigger = new CronTrigger("trigger"+CHGCODE, "groupControl", "job"+CHGCODE, "groupControl", pollVal);//����job��ѵ�����Լ�������Ϣ
        	trigger.setStartTime(DateUtil.parse3(infoHT.get("CHGSTART").toString()));//����job��ʼʱ��
        	sched.addJob(job, true);
        	sched.scheduleJob(trigger);
        	
    		System.out.println("Jobs "+CHGCODE+" start time��"+infoHT.get("CHGSTART")+"  pollingCycle��"+infoHT.get("CHGVALUE")+" "+infoHT.get("CHGFRQ"));
        }
		sched.start();
		System.out.println("***** Scheduling Jobs Started *****");
	}
	
	public static void main(String[] args){
		
		try {
			PropertysUtil pu = new PropertysUtil();
			Trigger trigger = new Trigger();
			
			pu.initPropertysUtil();
			
			trigger.run();
		} catch (Exception e) {
			e.printStackTrace();
		}
		
	}
}
