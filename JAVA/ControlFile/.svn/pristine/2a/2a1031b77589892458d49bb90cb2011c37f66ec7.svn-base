package com.task.bean;

import java.lang.reflect.Constructor;
import java.lang.reflect.Method;

import org.quartz.Job;
import org.quartz.JobExecutionContext;
import org.quartz.JobExecutionException;

import com.task.common.CalendarUtil;
import com.task.common.DateUtil;
import com.task.common.PropertysUtil;
import com.task.db.DBOperator;

public class invokeBean implements Job{

	private DBOperator dbo = new DBOperator();
	
	/**
	 * 根据配置档执行相应方法
	 * @param cmd
	 * @param methodName
	 */
	public void invokeMethod(String cmd,String methodName){
		try{
			Class cl = Class.forName(cmd);
//			检索构造方法
            Class[] strArgsClass = new Class[] {};
            Constructor constructor = cl.getConstructor(strArgsClass);
//          调用构造方法创建实例对象object
            Object[] strArgs = new Object[] {};
            Object object = constructor.newInstance(strArgs);
            
            Method meth = cl.getMethod(methodName, new Class[]{});
            meth.invoke(object, new Object[]{});
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	
	public void test(){
		try{
			String cmd = "com.task.test.test.test2";
			Class cl = Class.forName(cmd.substring(0,cmd.lastIndexOf(".")));
//			检索构造方法
            Class[] strArgsClass = new Class[] {};
            Constructor constructor = cl.getConstructor(strArgsClass);
//          调用构造方法创建实例对象object
            Object[] strArgs = new Object[] {};
            Object object = constructor.newInstance(strArgs);
//            boolean b1 = cl.isInstance(object); //ownclass
            
//            Method meth = cl.getMethod("test2", String.class);
            Method meth = cl.getMethod(cmd.substring(cmd.lastIndexOf(".")+1,cmd.length()), new Class[]{});
//            meth.invoke(object, "aa");
            meth.invoke(object, new Object[]{});
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	public void execute(JobExecutionContext context) throws JobExecutionException {
		 String startTime = context.getJobDetail().getJobDataMap().getString("CHGSTART");//交换开始时间
		 String endTime = context.getJobDetail().getJobDataMap().getString("CHGEND");//交换截止时间
		 String frq = context.getJobDetail().getJobDataMap().getString("CHGFRQ");//交换频率
		 String frqVal = context.getJobDetail().getJobDataMap().getString("CHGVALUE");//交换频率值 D:天 H:小时 M:分钟 S:秒
		 String cmd = context.getJobDetail().getJobDataMap().getString("COMMAND");//启动命令
		 String code = context.getJobDetail().getJobDataMap().getString("CHGCODE");//交换代码
		 
		 String nextTime = "";//下次运行时间
		 if(frq.equals("D")){
			 nextTime = DateUtil.getAddDate(DateUtil.formatDate3(DateUtil.getCurrentDate()), Integer.parseInt(frqVal), 0);
		 }else  if(frq.equals("H")){
			 nextTime = DateUtil.getAddHour(DateUtil.formatDate3(DateUtil.getCurrentDate()), Integer.parseInt(frqVal), 0);
		 }else  if(frq.equals("M")){
			 nextTime = DateUtil.getAddMinutes(DateUtil.formatDate3(DateUtil.getCurrentDate()), Integer.parseInt(frqVal), 0);
		 }else  if(frq.equals("S")){
			 nextTime = DateUtil.getAddSeconds(DateUtil.formatDate3(DateUtil.getCurrentDate()), Integer.parseInt(frqVal), 0);
		 }

		 String methodName = cmd.substring(cmd.lastIndexOf(".")+1,cmd.length());
		 cmd = cmd.substring(0,cmd.lastIndexOf("."));
		 
		 System.out.println("class="+cmd+" method="+methodName+" start="+DateUtil.formatDate3(DateUtil.getCurrentDate())+" endTime = "+endTime+"  nextTime = "+nextTime);
		 
		 if(CalendarUtil.compareDateTime(DateUtil.formatDate3(DateUtil.getCurrentDate()), endTime)==1){
			 System.out.println("start invoke ! "+code);
			 this.invokeMethod(cmd, methodName);
			 String sql = "update CHGCTL set NEXTDATE=to_date(?,'yyyy-mm-dd hh24:mi:ss') where CHGCODE=?";
			 Object[] obj = new Object[2];
			 obj[0] = nextTime;
			 obj[1] = code;
			 dbo.execData(PropertysUtil.getConnectInfo(), sql, obj);
		 }else{
			 System.out.println("the job "+code+" is end at = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
		 }
	}

	public static void main(String[] args){
		invokeBean invoke = new invokeBean();
		invoke.test();
//		String a = "com.task.test.test.test2";
//		System.out.println(a.substring(0,a.lastIndexOf(".")));
//		System.out.println(a.substring(a.lastIndexOf(".")+1,a.length()));
	}
}
