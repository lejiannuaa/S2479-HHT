package com.hola.common.mq;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;

import com.hola.common.exception.ReceiveMqException;
import com.hola.common.exception.SendMqException;
import com.hola.common.util.Uuid16;
import com.ibm.mq.MQC;
import com.ibm.mq.MQException;
import com.ibm.mq.MQMessage;
import com.ibm.mq.MQPutMessageOptions;
import com.ibm.mq.MQQueue;
import com.ibm.mq.MQQueueManager;



public class MqHelper implements IMqHelper
{
	@SuppressWarnings("deprecation")
	public static final int MQ_QOPTION_INPUT	=	MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_OUTPUT; 	//接收时使用
    @SuppressWarnings("deprecation")
	public static final int	MQ_QOPTION_OUTPUT	=	MQC.MQOO_INQUIRE | MQC.MQOO_OUTPUT; 		//发送时使用	
	
	private		String		host;
	private		Integer		port;
	private		String		transport = "MQSeries";
	private		String		channel = "SYSTEM.DEF.SVRCONN";
	private		Integer		CCSID = 1208;
	private		String		username;
	private		String		password;
	private		String		qMName;
	private		String		qName;
	
	class MQHouse
	{
		MQQueueManager	queueManager;
		MQQueue			queue;
	}
	
	public MqHelper(String host, Integer port, String username,
			String password, String qMName , String qName)
	{
		super();
		this.host = host;
		this.port = port;
		this.username = username;
		this.password = password;
		this.qMName = qMName;
		this.qName = qName;
	}
	/**
	 * 构建queueManager和queue，并打开连接
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-5 下午4:28:39
	 * @throws MQException
	 */
	@SuppressWarnings({ "unchecked", "rawtypes" })
	public MQHouse connect(int qOption) throws MQException
	{
		Hashtable properties = new Hashtable();
		properties.put("transport", transport);
		properties.put("channel", channel);
		properties.put("port", port);
		properties.put("CCSID", CCSID);
		properties.put("hostname", host);
		properties.put("username", username);
		properties.put("password", password);
//		properties.put("hostname", "10.130.1.119");
//		properties.put("username", "truser");
//		properties.put("password", "Testrite123");
		MQHouse mqHouse = new MQHouse();
		mqHouse.queueManager = new MQQueueManager(qMName, properties);
//		mqHouse.queueManager = new MQQueueManager("QM_HLC", properties);
		
		mqHouse.queue = mqHouse.queueManager.accessQueue(qName, qOption);
//		mqHouse.queue = mqHouse.queueManager.accessQueue("HLC_OUTBOUND_MDC", qOption);
		return mqHouse;
	}
	/**
	 * 1、构建并打开连接
	 * 2、写入消息
	 * 3、关闭连接销毁
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-5 下午4:28:35
	 * @param str
	 * @throws SendMqException 
	 * @throws IOException
	 * @throws MQException
	 */
	public void sendString(String str) throws SendMqException
	{
		MQHouse mq = null;
		try {
			mq = connect(MqHelper.MQ_QOPTION_OUTPUT);
			MQMessage putMessage = new MQMessage();
			putMessage.writeUTF(str);
			//设置写入消息的属性（默认属性）
	        MQPutMessageOptions pmo = new MQPutMessageOptions();
	        mq.queue.put(putMessage, pmo);
	        close(mq);
		} catch (MQException e) {
			SendMqException se = new SendMqException("连接时异常!!!");
			e.printStackTrace();
			throw se;
		} catch (IOException e) {
			SendMqException se = new SendMqException("发送时异常!!!");
			e.printStackTrace();
			throw se;
		} finally {
			try {
				close(mq);
			} catch (MQException e) {
				e.printStackTrace();
			}
		}
	}
	public synchronized List<File> receivFileForZip(File directory , String storeNo) throws ReceiveMqException
	{
		List<File> files = new ArrayList<File>();
		try {
			MQHouse house = connect(MqHelper.MQ_QOPTION_INPUT);
			while(true)
			{
				MQMessage getMessage = fetchOneMsg(house.queue);
				if(getMessage == null)
					break;
				
				byte[] fileData = new byte[getMessage.getDataLength()];   
				getMessage.readFully(fileData);
				File file = File.createTempFile(Uuid16.create().toString(), "csv", directory);
				FileOutputStream fops = new FileOutputStream(file);
				fops.write(fileData);
				fops.flush();
				fops.close();
				files.add(file);
			}
			this.close(house);
		} catch (MQException e) {
			ReceiveMqException re = new ReceiveMqException("连接MQ时异常!!!");
			e.printStackTrace();
			throw re;
		} catch (IOException e) {
			ReceiveMqException re = new ReceiveMqException("读取或写文件时异常!!!");
			e.printStackTrace();
			throw re;
		}
		
		return files;
	}
	
	public synchronized void sendFile(File file) throws SendMqException
	{
		MQHouse mq = null;
		try {
			mq = connect(MqHelper.MQ_QOPTION_OUTPUT);
		} catch (MQException e1) {
			e1.printStackTrace();
			SendMqException sme = new SendMqException("连接MQ时异常:" + this.getMqDetail());
			throw sme;
		}
		MQMessage putMessage = new MQMessage();
		try {
			putMessage.setStringProperty("MSG_ID", file.getName());
			FileInputStream fis = new FileInputStream(file);
			byte [] b = new byte [(int) file.length()];
			putMessage.write(b);
			fis.close();
			MQPutMessageOptions pmo = new MQPutMessageOptions();
			mq.queue.put(putMessage, pmo);
		} catch (FileNotFoundException e) 
		{
			e.printStackTrace();
			SendMqException sme = new SendMqException("读取文件时异常,文件路径:" + file.getPath());
			throw sme;
		} catch (IOException e) 
		{
			e.printStackTrace();
			SendMqException sme = new SendMqException("构建MQMessage时异常,文件路径:" + file.getPath());
			throw sme;
		} catch (MQException e) {
			e.printStackTrace();
			SendMqException sme = new SendMqException("发送MQ时异常,文件路径:" + file.getPath());
			throw sme;
		}
		try {
			this.close(mq);
		} catch (MQException e) {
			e.printStackTrace();
			SendMqException sme = new SendMqException("关闭MQ时异常,文件路径:" + file.getPath() + "MQ:" + this.getMqDetail());
			throw sme;
		}
	}
	/**  
     * 取出一个消息  
     *   
     * @param q  
     *            ?列名称  
     * @return  
     * @throws Exception  
     */  
    private MQMessage fetchOneMsg(MQQueue q)
    {   
//        MQGetMessageOptions mgo = new MQGetMessageOptions();   
//        mgo.options |= MQC.MQGMO_NO_WAIT;
        MQMessage msg = new MQMessage();   
        try {
        	q.get(msg);   
        } catch (MQException e) {   
            return null;   
        }   
        return msg;   
    }   
	/**
	 * 关闭连接，释放资源
	 * @author 王成(chengwangi@isoftstone.com)
	 * @param mq 
	 * @date 2013-1-5 下午4:29:01
	 * @throws MQException
	 */
	public void close(MQHouse mq) throws MQException
	{
		mq.queue.close();
		mq.queueManager.disconnect();
	}

	public String getHost() {
		return host;
	}

	public void setHost(String host) {
		this.host = host;
	}

	public Integer getPort() {
		return port;
	}

	public void setPort(Integer port) {
		this.port = port;
	}

	public Integer getCCSID() {
		return CCSID;
	}

	public void setCCSID(Integer cCSID) {
		CCSID = cCSID;
	}

	public String getUsername() {
		return username;
	}

	public void setUsername(String username) {
		this.username = username;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}
	public String getMqDetail()
	{
		return "Host:" + this.host + "Port:" + this.port + "MqManager:" + this.qMName + "QName:" + this.qName + "Username:" + this.username + "Password:" + this.password;
	}
	public static void main(String[] args) 
	{
//		#username=
//				#password=
//				#receiveTimeout=2000
//				#transportType=1
//				#hostName=
//				#CCSID=1208
//				#QUEUEMANAGER=
//				#PORT=

//		MqHelper mqHelper = new MqHelper("10.254.143.160", 1421, "truser", "Trgmq002", "QM_HLC", "HLC_PO2HHT_S13107");
//		try {
//			mqHelper.sendFile(new File("E:\\log\\2013.1.10_13107\\1\\HLC_PO2HHT_S13107_20130110000000000004_D16.txt"));
//		} catch (SendMqException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
//		int openOptions = MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_OUTPUT;
//		System.out.println(openOptions);
	}
}
