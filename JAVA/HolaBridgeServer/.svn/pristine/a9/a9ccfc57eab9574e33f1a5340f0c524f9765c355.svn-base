package com.hola.bs.core.mail;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Properties;
import java.util.ResourceBundle;

import javax.mail.BodyPart;
import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.Multipart;
import javax.mail.Session;
import javax.mail.internet.AddressException;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeBodyPart;
import javax.mail.internet.MimeMultipart;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

public class MailUtils {
	private static Log log = LogFactory.getLog(MailUtils.class);
	/** 资源文件 */
	public static ResourceBundle bundle = ResourceBundle.getBundle("mail");

	/**
	 * 发送mail
	 */
	public static boolean sendMail(Map<String, String> content) {
		Properties props = System.getProperties();
		props.put("mail.smtp.host", bundle.getString("mail.smtp"));
		props.put("mail.smtp.port", bundle.getString("mail.port"));
		// session
		javax.mail.Session session = javax.mail.Session.getInstance(props, null);
		Message msg = new javax.mail.internet.MimeMessage(session);
		try {
			msg.setFrom(new InternetAddress(bundle.getString("mail.from")));
			msg.setRecipients(Message.RecipientType.TO, InternetAddress.parse(bundle.getString("mail.to")));
			msg.setRecipients(Message.RecipientType.CC, InternetAddress.parse(bundle.getString("mail.cc")));
			msg.setSubject(bundle.getString("mail.subject"));
			// multipart
			BodyPart html = new MimeBodyPart();
			Iterator<String> iter = content.keySet().iterator();
			String template = getMailTemplate();
			while(iter.hasNext()){
				String key = iter.next();
				template = template.replace("${" + key +"}", content.get(key));
			}
			html.setContent(template, "text/html;charset=utf8");

			Multipart multiPart = new MimeMultipart();
			multiPart.addBodyPart(html);
			//
			msg.setContent(multiPart);
			msg.setSentDate(new Date());
			javax.mail.Transport.send(msg);
			log.info("邮件发送成功");
			return true;
		} catch (AddressException e) {
			log.error("邮件发送错误：", e);
		} catch (MessagingException e) {
			log.error("邮件发送错误：", e);
		}
		return false;
	}

	public static String getMailTemplate() {
		File mailTemplate = new File(MailUtils.class.getResource("/").getPath() + "/mail.template");
		StringBuffer template = new StringBuffer(); 
		try {
			BufferedReader in = new BufferedReader(new InputStreamReader(new FileInputStream(mailTemplate), "UTF-8"));
			String line; 
			while ((line = in.readLine()) != null) {
				template.append(line); 
				template.append("\n"); 
			}
			in.close();
		} catch (IOException e) {
			log.error("邮件模板读取错误", e);
		}
		return template.toString();
	}
	
	public  void test() {
		Map<String, String> map = new HashMap<String, String>();
		map.put("content", "test");
		sendMail(map);
	}
	public static void main(String[] args) {
		sendMail("1111111","sdsfsf");
	}
	
	
	public static boolean sendMail(String subject, String str) {

		Properties props = System.getProperties();
		props.put("mail.smtp.host", bundle.getString("mail.smtp"));
		props.put("mail.smtp.port", bundle.getString("mail.port"));

		Session session = Session.getInstance(props, null);
		Message msg = new javax.mail.internet.MimeMessage(session);
		try {
			msg.setFrom(new InternetAddress(bundle.getString("mail.from")));
			msg.setRecipients(Message.RecipientType.TO, InternetAddress.parse(bundle.getString("mail.to") , false));
			msg.setRecipients(Message.RecipientType.CC, InternetAddress.parse(bundle.getString("mail.cc") , false));
			msg.setSubject(subject);

			BodyPart html = new MimeBodyPart();
			html.setContent(str, "text/html;charset=utf8");

			Multipart multiPart = new MimeMultipart();
			multiPart.addBodyPart(html);

			msg.setContent(multiPart);
			msg.setSentDate(new Date());
			javax.mail.Transport.send(msg);
			log.info("邮件发送成功");
			return true;
		} catch (AddressException e) {
			log.error("邮件发送错误：", e);
		} catch (MessagingException e) {
			log.error("邮件发送错误：", e);
		}
		return false;
	}
}
