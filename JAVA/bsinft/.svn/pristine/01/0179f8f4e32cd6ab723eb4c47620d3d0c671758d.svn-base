package com.hola.tarjda;

import java.io.IOException;
import java.io.InputStream;
import java.util.Date;
import java.util.List;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import com.hola.common.ConfigHelper;
import com.hola.common.SpringContextHelper;
import com.hola.common.util.MailUtils;
import com.hola.common.util.ThreadPoolUtil;
import com.hola.common.util.TimeUtil;
import com.hola.tarjda.cache.ConfigCache;
import com.hola.tarjda.service.TarJdaService;

public class GetDataForBsSendMq 
{
	public static int instnoSum = 0;		//执行的批次数量计数器
	private static Log log = LogFactory.getLog(GetDataForBsSendMq.class);
	/**
	 * 获取数据组装csv发送MQ的入口.
	 * 会扫描所有hhtserver中所有的门店
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-8 下午5:33:43
	 */
	@SuppressWarnings("static-access")
	public void createDataToCsvSendMqAllStore()
	{
		instnoSum = 0;
		log.info("BS数据推送MQ程序启动!!!...");
		ThreadPoolUtil.initialize();
		SpringContextHelper context = SpringContextHelper.getInstance();
		log.info("获取spring环境成功");
		TarJdaService service = (TarJdaService) context.getBean("tarJdaService");
		log.info("获取service成功");
		List<String> allStoreNo = service.getStoreNoList();
		log.info("将要执行门店有..." + allStoreNo);
		int status = 0;
		for (String storeNo : allStoreNo) 
		{
			try {
				log.info("执行" + storeNo + "开始...");
				service.createCsvString(storeNo);
			} catch (Exception e) 
			{
				e.printStackTrace();
				log.error("执行" + storeNo + "时出错!!!" , e);
				status = -1;
				MailUtils.sendMail("执行" + storeNo + "时出错!!!" + e.getMessage());
				continue;
			}
		}
		
		while(ThreadPoolUtil.isAllCompleted(instnoSum) == false)
		{
			try {
				Thread.currentThread().sleep(2000);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
		shut(status);
	}
	private	void shut(int status)
	{
		ThreadPoolUtil.awaitTermination();
		ThreadPoolUtil.shutdown();
		ThreadPoolUtil.tpe = null;
		log.info("BS数据推送MQ程序运行结束!!!关闭");
//		System.exit(status);
	}
	public static void main(String[] args)
	{
		try {
			while (true) 
			{
				while (true) 
				{
					Date startDate = ConfigHelper.getInstance().getDateValue("startDateTime", TimeUtil.DATE_FORMAT_yyyyMMddHHmmss);
					if (new Date().getTime() >= startDate.getTime())
						break;
					Thread.sleep(1000);
				}
				new GetDataForBsSendMq().createDataToCsvSendMqAllStore();
				ConfigCache.getInstance().clearCache();
				System.gc();
				Thread.sleep(ConfigHelper.getInstance().getIntValue("intervalMinute") * 60000);
			}
		} catch (Exception e) 
		{
			e.printStackTrace();
			InputStream in = ConfigHelper.class.getClassLoader().getResourceAsStream("jdbc.properties");
			StringBuffer strBuf = new StringBuffer();
			byte [] b = new byte [512];
			try {
				while(in.read(b) != -1)
				{
					String str = new String(b);
					strBuf.append(str);
				}
			} catch (IOException e1) {
				e1.printStackTrace();
			}
			MailUtils.sendMail(strBuf.toString());
		}
		
		
	}
}
