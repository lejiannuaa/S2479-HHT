package com.hola.bs.pool;


import org.xsocket.connection.INonBlockingConnection;

import com.hola.bs.core.Command;

import edu.emory.mathcs.backport.java.util.concurrent.ArrayBlockingQueue;
import edu.emory.mathcs.backport.java.util.concurrent.BlockingQueue;
import edu.emory.mathcs.backport.java.util.concurrent.ThreadFactory;
import edu.emory.mathcs.backport.java.util.concurrent.ThreadPoolExecutor;
import edu.emory.mathcs.backport.java.util.concurrent.TimeUnit;


public class ThreadFactoryMBImpl {

	private static ThreadPoolExecutor tpe = null;
	
	public static void runJobTestClientSend(int i){
        tpe.submit(new Runner(i));
    }
	
	public static void runJob(Command command,INonBlockingConnection nbc){
		tpe.submit(new Runner(command,nbc));
	}

    public static void shutDownPool(){
    	try{
    		if(tpe != null){
    			tpe.shutdown();
    			tpe.awaitTermination(10, TimeUnit.SECONDS);
//    			logger.info("Thread Pool shut down");
    		}
    	}catch(InterruptedException e){
    		e.printStackTrace();
//    		throw new LoggableEJBException(e,"02001");
    	}
    }
    
    public static void initialize(){
    	try{
    		if(tpe==null){
	            BlockingQueue q = new ArrayBlockingQueue(1000);
	            ThreadFactory tf = new ThreadFactoryImpl();
	            tpe = new ThreadPoolExecutor(50, 400, 2000, TimeUnit.SECONDS, q, tf);
//	            logger.info("create Thread Factory");
    		}
    	}catch(Exception e){
    		e.printStackTrace();
//    		throw new LoggableEJBException(e,"02000");
    	}
    }
}
