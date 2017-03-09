package com.hola.jda2hht.util;

import java.io.BufferedInputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Hashtable;
import java.util.List;

import com.ibm.mq.MQC;
import com.ibm.mq.MQException;
import com.ibm.mq.MQMessage;
import com.ibm.mq.MQPutMessageOptions;
import com.ibm.mq.MQQueue;
import com.ibm.mq.MQQueueManager;

public class MqHelper {
	private String host;
	private Integer port;
	private String transport = "MQSeries";
	private String channel = "SYSTEM.DEF.SVRCONN";
	private Integer ccsid;
	private String username;
	private String password;
	private String qMName;
	private String qName;
	private MQQueueManager queueManager;
	private MQQueue queue;

	public MqHelper(String host, Integer port, String username,
			String password, String qMName, String qName, Integer ccsid) {
		super();
		this.host = host;
		this.port = port;
		this.username = username;
		this.password = password;
		this.qMName = qMName;
		this.qName = qName;
		this.ccsid = ccsid;
	}

	/**
	 * 构建queueManager和queue，并打开连接
	 * 
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-5 下午4:28:39
	 * @throws MQException
	 */
	@SuppressWarnings({ "unchecked", "rawtypes" })
	public void connect() throws MQException {
		Hashtable properties = new Hashtable();
		properties.put("hostname", host);
		properties.put("transport", transport);
		properties.put("channel", channel);
		properties.put("port", port);
		properties.put("CCSID", ccsid);
		properties.put("username", username);
		properties.put("password", password);
		this.queueManager = new MQQueueManager(qMName, properties);
		// int openOptions = MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_OUTPUT;
		int openOptions = MQC.MQOO_OUTPUT | MQC.MQOO_FAIL_IF_QUIESCING;
		this.queue = queueManager.accessQueue(qName, openOptions);
	}

	/**
	 * 1、构建并打开连接 2、写入消息 3、关闭连接销毁
	 * 
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-5 下午4:28:35
	 * @param str
	 * @throws IOException
	 * @throws MQException
	 */
	public void sendString(String str) throws IOException, MQException {
		connect();
		MQMessage putMessage = new MQMessage();
		putMessage.writeUTF(str);
		// 设置写入消息的属性（默认属性）
		MQPutMessageOptions pmo = new MQPutMessageOptions();
		this.queue.put(putMessage, pmo);
		close();
	}

	public synchronized List<File> receivFile(File directory)
			throws MQException, IOException {
		// List<File> files = new ArrayList<File>();
		// connect();
		// while (true) {
		// MQMessage getMessage = fetchOneMsg(this.queue);
		// if (getMessage == null)
		// break;
		//
		// byte[] fileData = new byte[getMessage.getDataLength()];
		// getMessage.readFully(fileData);
		// File file = File.createTempFile(Uuid16.create().toString(), "txt",
		// directory);
		// FileOutputStream fops = new FileOutputStream(file);
		// fops.write(fileData);
		// fops.close();
		// files.add(file);
		// }
		// this.close();
		// return files;
		return null;
	}

	/**
	 * 发送文件
	 * 
	 * @file: MqHelper.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 * @param file
	 * @throws MQException
	 * @throws IOException
	 */
	public void sendFile(File file) throws MQException, IOException {
		MQMessage putMessage = new MQMessage();
		InputStream is = null;
		try {
			is = new BufferedInputStream(new FileInputStream(file));
			byte[] buff = new byte[1024];
			while (is.read(buff) != -1) {
				putMessage.write(buff);
			}
			this.queue.put(putMessage);
		} finally {
			is.close();
		}
	}

	// /**
	// * 取出一个消息
	// *
	// * @param q
	// * ?列名称
	// * @return
	// * @throws Exception
	// */
	// private MQMessage fetchOneMsg(MQQueue q) {
	// MQGetMessageOptions mgo = new MQGetMessageOptions();
	// mgo.options |= MQC.MQGMO_NO_WAIT;
	// MQMessage msg = new MQMessage();
	// try {
	// q.get(msg, mgo);
	// } catch (MQException e) {
	// return null;
	// }
	// return msg;
	// }

	/**
	 * 关闭连接，释放资源
	 * 
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-5 下午4:29:01
	 * @throws MQException
	 */
	public void close() throws MQException {
		queue.close();
		queueManager.disconnect();
	}

	// public static void main(String[] args) {
	// File file = new File("D:\\dataCheckForTarHHT.jar");
	// System.out.println(file);
	// try {
	// FileInputStream fis = new FileInputStream(file);
	// byte[] b = new byte[(int) file.length()];
	// fis.read(b);
	// System.out.println(b);
	//
	// } catch (FileNotFoundException e) {
	// // TODO Auto-generated catch block
	// e.printStackTrace();
	// } catch (IOException e) {
	// // TODO Auto-generated catch block
	// e.printStackTrace();
	// }
	//
	// // try {
	// // File.createTempFile("com.ibm.mq.tools.ras", "jar", new File("D:\\"));
	// // } catch (IOException e) {
	// // // TODO Auto-generated catch block
	// // e.printStackTrace();
	// // }
	// // File f = new File("D:\\nnn\\com.ibm.mq.tools.ras.jar");
	// // File newF = new File("D:\\com.ibm.mq.tools.ras.jar");
	// // try {
	// // FileInputStream fis = new FileInputStream(f);
	// // byte [] b = new byte [(int) f.length()];
	// // try {
	// // fis.read(b);
	// // } catch (IOException e) {
	// // // TODO Auto-generated catch block
	// // e.printStackTrace();
	// // }
	// //
	// // FileOutputStream fos = new FileOutputStream(newF);
	// // try {
	// // fos.write(b);
	// // } catch (IOException e) {
	// // // TODO Auto-generated catch block
	// // e.printStackTrace();
	// // }
	// //
	// // } catch (FileNotFoundException e) {
	// // // TODO Auto-generated catch block
	// // e.printStackTrace();
	// // }
	// }
}
