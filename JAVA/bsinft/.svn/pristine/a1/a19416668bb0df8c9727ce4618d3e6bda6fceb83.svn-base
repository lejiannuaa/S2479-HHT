package com.hola.common.util;

import java.util.concurrent.ArrayBlockingQueue;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.Executors;
import java.util.concurrent.ThreadFactory;
import java.util.concurrent.ThreadPoolExecutor;
import java.util.concurrent.TimeUnit;

import org.apache.log4j.Logger;

import com.hola.common.ConfigHelper;

/**
 * 线程池的简单封装，工具类
 * 
 * @author 唐植超(上海软通)
 * @date 2013-1-8
 */
public class ThreadPoolUtil {

	private static Logger log = Logger.getLogger(ThreadPoolUtil.class);
	public static ThreadPoolExecutor tpe = null;

	public static boolean isAllCompleted(int count)
	{
		if(tpe.getCompletedTaskCount() >= count)
			return true;
		return false;
	}
	/**
	 * 启动线程
	 * 
	 * @file: ThreadFactoryMBImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-8
	 * @param run
	 */
	public static void runJob(Runnable run) {
		tpe.submit(run);
	}

	/**
	 * 停止线程
	 * 
	 * @file: ThreadFactoryMBImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 */
	public static void awaitTermination() {
		try {
			if (tpe != null) {
				// tpe.shutdown();
				tpe.awaitTermination(100, TimeUnit.MILLISECONDS);
				log.info("Thread Pool shut down");
			}
		} catch (InterruptedException e) {
			e.printStackTrace();
			log.error(e.getMessage() , e);
		}
	}

	/**
	 * 退出线程池
	 * 
	 * @file: ThreadPoolUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-8
	 */
	public static void shutdown() {
		if (tpe != null) {
			tpe.shutdown();
		}
	}

	/**
	 * 初始化线程池
	 * 
	 * @file: ThreadFactoryMBImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 */
	public static void initialize() 
	{
		initialize(ConfigHelper.getInstance().getValue(ConfigHelper.THREADPOOL_MAX_POOL_SIZE) , 
				ConfigHelper.getInstance().getValue(ConfigHelper.THREADPOOL_CORE_POOL_SIZE) , 
				ConfigHelper.getInstance().getValue(ConfigHelper.THREADPOOL_QUEUE_SIZE));
	}
	
	/**
	 * 初始化线程池
	 * 
	 * @file: ThreadFactoryMBImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 */
	@SuppressWarnings({ "rawtypes", "unchecked" })
	public static void initialize(String maxPS , String corePS , String qs) 
	{
		try {
			if (tpe == null) {
				log.info("根据配置构建线程池");
				int queueSize = Integer.parseInt(qs);
				// 队列
				BlockingQueue q = new ArrayBlockingQueue(queueSize);
				// 线程工厂
				ThreadFactory tf = Executors.defaultThreadFactory();
				// 线程池
				int corePoolSize = Integer.parseInt(corePS);
				int maxPoolSize = Integer.parseInt(maxPS);
				int keepAlvieTime = Integer.parseInt(ConfigHelper.getInstance().getValue(ConfigHelper.THREADPOOL_KEEP_ALVIE_TIME));
				tpe = new ThreadPoolExecutor(corePoolSize, maxPoolSize,
						keepAlvieTime, TimeUnit.SECONDS, q, tf);
				log.info("create Thread Factory");
			}
		} catch (Exception e) {
			e.printStackTrace();
			log.error(e.getMessage() , e);
		}
	}
	
	public static void main(String[] args) 
	{
		ThreadPoolUtil.initialize("10", "10", "10");
		for (int i = 0; i < 30; i++) 
		{
			TestRun r = new TestRun(i + "");
			ThreadPoolUtil.runJob(r);
		}
		
		System.out.println("开始销毁");
		long t = System.currentTimeMillis();
		ThreadPoolUtil.awaitTermination();
		long t1 = System.currentTimeMillis();
		System.out.println(t1 - t);
		ThreadPoolUtil.shutdown();
		System.out.println(System.currentTimeMillis() - t1);
		System.out.println("已经销毁");
	}
	
	
}

class TestRun implements Runnable
{
	String name = "";
	public TestRun(String name)
	{
		this.name = name;
	}
	@Override
	public void run() 
	{
		System.out.println("执行中...." + name);
	}
}
