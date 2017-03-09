package service;

import java.util.List;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

import model.IntfInfo;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import common.ConfigHelper;


public class IntfMonitor {
	
	private static Log log = LogFactory.getLog(IntfMonitor.class);

    public static void main(String[] args) {  
        Runnable runnable = new RunThread();
        
        String str_timing = ConfigHelper.getInstance().getValue("schedule.timing");
        
        try {
            int timing = Integer.parseInt(str_timing);

            ScheduledExecutorService service = Executors  
                    .newSingleThreadScheduledExecutor();
            service.scheduleAtFixedRate(runnable, 10, timing, TimeUnit.SECONDS);  
            
        } catch (NumberFormatException e) {
            e.printStackTrace();
        }
        
    }  
    

    public static class RunThread implements Runnable{
    	
    	public RunThread(){
    		
    	}
    	
		@Override
		public void run() {
			// TODO Auto-generated method stub
			//System.out.println("Hello !!");
			
			log.info("查询需要监控的interface");
			
			List<IntfInfo> intfInfoList = MonitorService.receiveDBForAllSystem();
			
			for(IntfInfo intfInfo : intfInfoList)
			{
				log.info("监控interface: " + intfInfo.getIntfName());
				MonitorService.monitortime(intfInfo);
			}
    		
		}
    	
    }
    
}
