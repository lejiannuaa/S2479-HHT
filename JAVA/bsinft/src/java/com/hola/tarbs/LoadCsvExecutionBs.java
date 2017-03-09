package com.hola.tarbs;

import java.io.IOException;
import java.io.InputStream;
import java.util.Date;
import java.util.List;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import com.hola.common.ConfigHelper;
import com.hola.common.SpringContextHelper;
import com.hola.common.model.mw.StoreInfo;
import com.hola.common.util.MailUtils;
import com.hola.common.util.TimeUtil;
import com.hola.tarbs.service.TarBsService;
import com.hola.tarjda.cache.ConfigCache;

public class LoadCsvExecutionBs 
{
	private static Log log = LogFactory.getLog(LoadCsvExecutionBs.class);
	public static long storeUseUp = 0l;
	public void receiveMqExecutionBsForAllStore()
	{
		TarBsService service = (TarBsService) SpringContextHelper.getInstance().getBean("tarBsService");
		log.info("jda下行，mq获取数据，保存BS程序开始.......");
		log.info("成功获取service");
		List<StoreInfo> storeInfoList = service.getStoreInfoList();
		log.info("成功获取所有门店信息:" + storeInfoList);
		for (StoreInfo storeInfo : storeInfoList) 
		{
			log.info("开始执行" + storeInfo.getStoreNo());
			try {
				storeUseUp = System.currentTimeMillis();
				service.getCsvfilesForMqSaveBs(storeInfo);
			} catch (Exception e) {
				e.printStackTrace();
				log.error("执行门店:" + storeInfo.getStoreNo() + "出错!!!" , e);
				MailUtils.sendMail("执行门店:" + storeInfo.getStoreNo() + "出错!!!" + e.getMessage());
			}
			log.info("完成:" + storeInfo.getStoreNo());
		}
		ConfigCache.getInstance().clearCache();
		log.info("jda下行，mq获取数据，保存BS程序结束!!!");
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
				new LoadCsvExecutionBs().receiveMqExecutionBsForAllStore();
				//System.exit(0);
				System.gc();
				Thread.sleep(ConfigHelper.getInstance().getIntValue("intervalMinute") * 60000);
			}
		} catch (Exception e) 
		{
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