package com.hola.jda2hht.util;

import java.util.Vector;
import java.util.concurrent.ArrayBlockingQueue;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.concurrent.ThreadFactory;
import java.util.concurrent.ThreadPoolExecutor;
import java.util.concurrent.TimeUnit;

import org.apache.log4j.Logger;

/**
 * 线程池的简单封装，工具类
 * 
 * @author 唐植超(上海软通)
 * @date 2013-1-8
 */
public class ThreadPoolUtil {

	private static Logger log = Logger.getLogger(ThreadPoolUtil.class);
	private static ThreadPoolExecutor tpe = null;
	private static Vector<Future<String>> list = new Vector<Future<String>>();

	/**
	 * 启动线程
	 * 
	 * @file: ThreadFactoryMBImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-8
	 * @param run
	 */
	public static void runJob(Callable<String> run) {
		list.add(tpe.submit(run));
	}

	/**
	 * 退出线程池
	 * 
	 * @file: ThreadPoolUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-8
	 */
	public static void shutdown() {
		for (Future<String> f : list) {
			try {
				log.info("线程执行完毕，批次号：" + f.get());
			} catch (InterruptedException e) {
				e.printStackTrace();
			} catch (ExecutionException e) {
				e.printStackTrace();
			}
		}
		tpe.shutdown();
		System.exit(0);
	}

	/**
	 * 初始化线程池
	 * 
	 * @file: ThreadFactoryMBImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 */
	@SuppressWarnings({ "rawtypes", "unchecked" })
	public static void initialize() {
		try {
			if (tpe == null) {
				log.info("根据配置构建线程池");
				int queueSize = Integer.parseInt(ConfigUtil
						.getConfig("thread_queue_size"));
				// 队列
				BlockingQueue q = new ArrayBlockingQueue(queueSize);
				// 线程工厂
				ThreadFactory tf = Executors.defaultThreadFactory();
				// 线程池
				int corePoolSize = Integer.parseInt(ConfigUtil
						.getConfig("corePoolSize"));
				int maxPoolSize = Integer.parseInt(ConfigUtil
						.getConfig("maxPoolSize"));
				int keepAlvieTime = Integer.parseInt(ConfigUtil
						.getConfig("keepAlvieTime"));
				tpe = new ThreadPoolExecutor(corePoolSize, maxPoolSize,
						keepAlvieTime, TimeUnit.SECONDS, q, tf);
				log.info("create Thread Factory");
			}
		} catch (Exception e) {
			e.printStackTrace();
			log.error(e.getMessage(), e);
		}
	}

}
