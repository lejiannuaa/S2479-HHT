package com.hola.common.mq;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import javax.jms.JMSException;
import javax.jms.Message;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.jms.connection.UserCredentialsConnectionFactoryAdapter;
import org.springframework.jms.core.JmsTemplate;
import org.springframework.jms.support.converter.SimpleMessageConverter;

import com.hola.common.ConfigHelper;
import com.hola.common.exception.ReceiveMqException;
import com.hola.common.exception.SendMqException;
import com.hola.common.file.FileManager;
import com.hola.common.util.ThreadPoolUtil;
import com.hola.common.util.Uuid16;
import com.hola.common.util.ZipUtils;
import com.ibm.mq.jms.MQQueue;

/**
 * JMS方式与MQ通讯的辅助类
 * @author 王成(chengwangi@isoftstone.com)
 * @date 2013-2-25 下午1:59:43
 */
public class JmsHelper implements IMqHelper
{
	private static Log log = LogFactory.getLog(JmsHelper.class);
	private		String		host;
	private		Integer		port;
	private		Integer		CCSID = 1208;
	private		String		username;
	private		String		password;
	private		String		qMName;
	private		String		qName;
	private		JmsTemplate	jmsTemplate;
	
	public JmsHelper(String host, Integer port, String username,
			String password, String qMName , String qName)
	{
		super();
		this.host = host;
		this.port = port;
		this.username = username;
		this.password = password;
		this.qMName = qMName;
		this.qName = qName;
		this.getInstance();
	}
	
	public void getInstance()
	{
		// 创建MQQueueConnectionFactory对象
		com.ibm.mq.jms.MQQueueConnectionFactory factory = new com.ibm.mq.jms.MQQueueConnectionFactory();

		try {
			factory.setTransportType(1);
			factory.setHostName(host);
			factory.setCCSID(CCSID);
			factory.setPort(port);
			factory.setQueueManager(qMName);

		} catch (NumberFormatException e1) {
			e1.printStackTrace();
		} catch (JMSException e1) {
			e1.printStackTrace();
		}

		// 建立Spring工厂对象
		UserCredentialsConnectionFactoryAdapter factoryAdapter = new UserCredentialsConnectionFactoryAdapter();
		factoryAdapter.setUsername(username);
		factoryAdapter.setPassword(password);
		factoryAdapter.setTargetConnectionFactory(factory);

		// 建立Spring JMSTemplate
		JmsTemplate template = new JmsTemplate();
		template.setConnectionFactory(factoryAdapter);
		template.setMessageConverter(new org.springframework.jms.support.converter.SimpleMessageConverter());
		template.setPubSubDomain(false);
		template.setReceiveTimeout(2000);
		template.setSessionTransacted(true);
		template.setSessionAcknowledgeMode(0);

		this.jmsTemplate = template;
	}
	public void sendFile(File file) throws SendMqException
	{
		sendFile(file, file.getName());
	}
	public synchronized void sendFile(List<File> fileList) throws SendMqException
	{
		try {
			log.info("开始MQ推送File操作");
			MQQueue queue = new MQQueue(qMName, qName);
			log.info("获取Queue,OK");
			for (File file : fileList) 
			{
				try
				{
					byte [] b = FileManager.getByteByFile(file);
					ByteMessageCreatorImpl bmc = new ByteMessageCreatorImpl(file.getName(), b);
					jmsTemplate.send(queue , bmc);
					log.info("发送File,OK");
					log.info(file.getPath() + "已发送:" + getMqDetail());
				} catch(Exception e)
				{
					e.printStackTrace();
				}
			}
		} catch (JMSException e) {
			log.error("jms发送时异常,发送的MQ：" + getMqDetail() , e);
			SendMqException sme = new SendMqException("发送文件时异常!");
			e.printStackTrace();
			throw sme;
		}
	}
	public void sendFile(File file , String msgid) throws SendMqException
	{
		try {
			log.info("开始MQ推送File操作");
			MQQueue queue = new MQQueue(qMName, qName);
			log.info("获取Queue,OK");
			byte [] b = FileManager.getByteByFile(file);
			ByteMessageCreatorImpl bmc = new ByteMessageCreatorImpl(msgid, b);
			jmsTemplate.send(queue , bmc);
			log.info("发送File,OK");
			log.info(file.getPath() + "已发送:" + getMqDetail());
		} catch (JMSException e) {
			log.error("jms发送时异常,file:" + file.getPath() + ".发送的MQ：" + getMqDetail() , e);
			SendMqException sme = new SendMqException("发送文件时异常!");
			e.printStackTrace();
			throw sme;
		} catch (IOException e) {
			log.error("获取文件字节时异常!,file:" + file.getPath(), e);
			SendMqException sme = new SendMqException("获取文件时异常!");
			e.printStackTrace();
			throw sme;
		}
	}
	
	public String getMqDetail()
	{
		return "Host:" + this.host + "Port:" + this.port + "MqManager:" + this.qMName + "QName:" + this.qName + "Username:" + this.username + "Password:" + this.password;
	}

//	@Override
//	public List<File> receivFileForZip(File directory , String storeNo) throws ReceiveMqException 
//	{
//		List<File> fileList = new ArrayList<File>();
//		try {
//			//SimpleMessageConverter messageConverter = (SimpleMessageConverter) jmsTemplate.getMessageConverter();
////			MQQueue queue = new MQQueue(qMName, qName);
//			while(true)
//			{
//				
//				String maxPS = ConfigHelper.getInstance().getValue(ConfigHelper.THREADPOOL_KEEP_ALVIE_TIME);
//				ThreadPoolUtil.initialize(maxPS, maxPS);
//				
//				
//				long fileUseUp = System.currentTimeMillis();
//				Message message = jmsTemplate.receive(qName);
//				if(message == null)
//					break;
//				String zipFileName = message.getStringProperty("MSG_ID");
//				zipFileName = getZipFileNameByMsgId(zipFileName);
//				log.info("从MQ获取文件" + zipFileName);
//				SimpleMessageConverter messageConverter = (SimpleMessageConverter) jmsTemplate.getMessageConverter();
//				Object oMsg = messageConverter.fromMessage(message);
//				
//				byte[] byteMessage = (byte[]) oMsg;
//				File folder = FileManager.getCsvFileDirectoryForMqToBs(storeNo);
//				File zipFile = new File(folder , zipFileName);
//				zipFile.createNewFile();
//				FileManager.writeByteToFile(zipFile, byteMessage);
//				log.info("拉取文件用时:" + (System.currentTimeMillis() - fileUseUp) + "文件大小:" + zipFile.length());
//				//解压
//				String csvFileName = new ZipUtils().unZipFileOne(zipFile, folder.getPath());
//				File csvFile = new File(folder , csvFileName);
//				log.info("解压后的文件:" + csvFile.getPath());
//				fileList.add(csvFile);
//				log.info("拉取文件和解压共计用时:" + (System.currentTimeMillis() - fileUseUp));
//			}
//		} catch (JMSException e) 
//		{
//			e.printStackTrace();
//		} catch (IOException e) 
//		{
//			e.printStackTrace();
//		}
//		return fileList;
//	}
	/**
	 * 多线程方式从MQ中拉取CSV文件，考虑内存问题，此方法增加同步限制
	 */
	@SuppressWarnings("static-access")
	@Override
	public synchronized List<File> receivFileForZip(File directory , String storeNo) throws ReceiveMqException 
	{
		
		List<File> fileList = new ArrayList<File>();
		String maxPS = ConfigHelper.getInstance().getValue(ConfigHelper.THREADPOOL_MAX_POOL_SIZE_RECEIV_MQ);
		ThreadPoolUtil.initialize(maxPS, maxPS , maxPS);			//初始化线程池
		FlagBit_receivFileForZip flagBit = new FlagBit_receivFileForZip();	//此对象为监控文件获取情况的标志位对象
		
//		long t = System.currentTimeMillis();
//		Message message = jmsTemplate.receive(qName);
//		System.out.println("reveive用时:" + (System.currentTimeMillis() - t));
//		return new ArrayList<File>();
		
		
		Runnable crt = new CreateRunThread(storeNo, fileList, flagBit);
		new Thread(crt).start();
		
		while(true)
		{
			if(flagBit.messageIsNull == true)//当前消息队列无消息可接时，等待线程池内线程Completed后，结束接收,销毁线程池
			{
				//循环观察所有线程已completed后，销毁线程池并返回文件列表
				while(ThreadPoolUtil.isAllCompleted(flagBit.threadCount) == false)
				{
					try {
						Thread.currentThread().sleep(300);
					} catch (InterruptedException e) {
						e.printStackTrace();
					}
				}
				log.info("调用awaitTermination");
				ThreadPoolUtil.awaitTermination();
				ThreadPoolUtil.shutdown();
				ThreadPoolUtil.tpe = null;
				log.info("获取文件总数:" + fileList.size() + "。mq信息:" + this.getMqDetail());
				return fileList;
			}
			try {
				Thread.sleep(300);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
	}
	class CreateRunThread implements Runnable
	{
		String storeNo;
		List<File> fileList;
		FlagBit_receivFileForZip flagBit;
		
		private CreateRunThread(String storeNo, List<File> fileList,
				FlagBit_receivFileForZip flagBit) 
		{
			this.storeNo = storeNo;
			this.fileList = fileList;
			this.flagBit = flagBit;
		}

		@SuppressWarnings("static-access")
		@Override
		public void run() 
		{
			while(flagBit.messageIsNull == false)
			{
				ReceivFileForZipRun run = new ReceivFileForZipRun(storeNo, fileList, flagBit);	//初始化执行器
				while(true)
				{
					try
					{
						ThreadPoolUtil.runJob(run);		//启动线程执行
						flagBit.threadCount ++;
						break;
					} catch(java.util.concurrent.RejectedExecutionException e)
					{
						if(flagBit.messageIsNull == true)
							return;
						log.info("mq拉取数据队列已满，MQ信息:" + JmsHelper.this.getMqDetail());
						try {
							Thread.currentThread().sleep(8000);
						} catch (InterruptedException e1) {
							e1.printStackTrace();
						}
						if(flagBit.messageIsNull == true)
							return;
					}
				}
			}
			
		}
		
	}
	class FlagBit_receivFileForZip
	{
		int threadCount = 0;			//拉取文件的线程计数器
		boolean messageIsNull = false;	//队列没有文件时，此属性为true
	}
	
	class ReceivFileForZipRun implements Runnable
	{
		String storeNo;
		List<File> fileList;
		FlagBit_receivFileForZip flagBit;
		
		public ReceivFileForZipRun(String storeNo, List<File> fileList,
				FlagBit_receivFileForZip flagBit) 
		{
			this.storeNo = storeNo;
			this.fileList = fileList;
			this.flagBit = flagBit;
		}
		
		@Override
		public void run() 
		{
			try
			{
				
				Thread.currentThread().setName(Uuid16.create().toString());
				long fileUseUp = System.currentTimeMillis();
				if(flagBit.messageIsNull == true)
					return;
				Message message = jmsTemplate.receive(qName);
				if(message == null)
				{
					flagBit.messageIsNull = true;
					return;
				}
				
				String zipFileName = message.getStringProperty("MSG_ID");
				zipFileName = getZipFileNameByMsgId(zipFileName);
				log.info("从MQ获取文件" + zipFileName);
				SimpleMessageConverter messageConverter = (SimpleMessageConverter) jmsTemplate.getMessageConverter();
				Object oMsg = messageConverter.fromMessage(message);
				
				byte[] byteMessage = (byte[]) oMsg;
				File folder = FileManager.getCsvFileDirectoryForMqToBs(storeNo);
				File zipFile = new File(folder , zipFileName);
				zipFile.createNewFile();
				FileManager.writeByteToFile(zipFile, byteMessage);
				log.info("拉取文件用时:" + (System.currentTimeMillis() - fileUseUp) + "文件大小:" + zipFile.length());
				//解压
				String csvFileName = new ZipUtils().unZipFileOne(zipFile, folder.getPath());
				File csvFile = new File(folder , csvFileName);
				log.info("解压后的文件:" + csvFile.getPath());
				fileList.add(csvFile);
				log.info("拉取文件和解压共计用时:" + (System.currentTimeMillis() - fileUseUp));
			} catch (JMSException e) {
				log.error("MQ拉去数据时失败,store:" + storeNo + "MQ信息:" + JmsHelper.this.getMqDetail() , e);
			} catch (IOException e) {
				log.error("MQ拉去数据保存文件时失败,store:" + storeNo + "MQ信息:" + JmsHelper.this.getMqDetail() , e);
			}
			
		}
	}
	
	private String getZipFileNameByMsgId(String zipFileName) 
	{
		zipFileName = zipFileName.toUpperCase();
		int index = zipFileName.indexOf(".CSV");
		if(index > 0)
			return zipFileName.substring(0, index) + ".zip";
		return zipFileName;
	}
}
