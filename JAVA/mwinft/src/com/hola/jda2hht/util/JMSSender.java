package com.hola.jda2hht.util;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;

import javax.jms.BytesMessage;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.Session;

import org.apache.log4j.Logger;
import org.springframework.jms.connection.UserCredentialsConnectionFactoryAdapter;
import org.springframework.jms.core.JmsTemplate;
import org.springframework.jms.core.MessageCreator;

import com.ibm.mq.jms.MQQueue;

public class JMSSender {

	private static final Logger log = Logger.getLogger(JMSSender.class);
	private JmsTemplate jmsTemplate;

	private String queueManagerName;

	public JMSSender(int transportType, String host, int ccsid,
			String queueManagerName, int port, long receiveTimeout,
			String userName, String passWord) {

		// 创建MQQueueConnectionFactory对象
		com.ibm.mq.jms.MQQueueConnectionFactory factory = new com.ibm.mq.jms.MQQueueConnectionFactory();

		try {
			factory.setTransportType(transportType);
			factory.setHostName(host);
			factory.setCCSID(ccsid);
			factory.setPort(port);
			factory.setQueueManager(queueManagerName);

		} catch (NumberFormatException e1) {
			e1.printStackTrace();
		} catch (JMSException e1) {
			e1.printStackTrace();
		}

		// 建立Spring工厂对象
		UserCredentialsConnectionFactoryAdapter factoryAdapter = new UserCredentialsConnectionFactoryAdapter();
		factoryAdapter.setUsername(userName);
		factoryAdapter.setPassword(passWord);
		factoryAdapter.setTargetConnectionFactory(factory);

		// 建立Spring JMSTemplate
		JmsTemplate template = new JmsTemplate();
		template.setConnectionFactory(factoryAdapter);
		template.setMessageConverter(new org.springframework.jms.support.converter.SimpleMessageConverter());
		template.setPubSubDomain(false);
		template.setReceiveTimeout(receiveTimeout);
		template.setSessionTransacted(true);
		template.setSessionAcknowledgeMode(0);

		// 建立JMSSender对象并设定Spring template
		this.jmsTemplate = template;
		this.queueManagerName = queueManagerName;

	}

	public void sendBinaryMsg(final File file, final String queueName)
			throws JMSException {
		try {
			MQQueue queue = new MQQueue(this.queueManagerName, queueName);

			this.jmsTemplate.send(queue, new MessageCreator() {

				@Override
				public Message createMessage(Session session)
						throws JMSException {
					BytesMessage msg = session.createBytesMessage();
					try {
						byte[] bytes = getBytesFromFile(file);
						msg.setStringProperty("MSG_ID", file.getName());
						log.info("----MSG_ID:" + file.getName());
						msg.writeBytes(bytes);
						return msg;
					} catch (Exception e) {
						e.printStackTrace();
					}
					return null;
				}
			});
			log.info("发送文件成功");
		} catch (JMSException e) {
			e.printStackTrace();
			throw e;
		} catch (Exception e) {
			e.printStackTrace();
			log.info(e.getMessage());
		}
	}

	/**
	 * 获得文件流
	 * 
	 * @file: JMSSender.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-11
	 * @param file
	 * @return
	 * @throws IOException
	 */
	private static byte[] getBytesFromFile(File file) throws IOException {
		if (file == null) {
			return null;
		}
		try {
			FileInputStream stream = new FileInputStream(file);
			ByteArrayOutputStream out = new ByteArrayOutputStream(1024);
			byte[] b = new byte[1024];
			int n;
			while ((n = stream.read(b)) != -1)
				out.write(b, 0, n);
			stream.close();
			out.close();
			return out.toByteArray();
		} catch (IOException e) {
			e.printStackTrace();
		}
		return null;
	}

	public static void main(String[] args) {
		JMSSender s = new JMSSender(1, "10.130.1.119", 1208, "QM_HLC", 1421,
				2000, "truser", "Testrite123");
		try {
			File file = new File("D:\\test.csv");
			s.sendBinaryMsg(file, "HLC_OUTBOUND_MDC");

		} catch (JMSException e) {
			e.printStackTrace();
		}
	}

}
