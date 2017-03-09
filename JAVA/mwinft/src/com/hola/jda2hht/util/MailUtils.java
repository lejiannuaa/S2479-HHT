package com.hola.jda2hht.util;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Properties;

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

	/**
	 * 发送mail
	 */
	public static boolean sendMail(Map<String, String> content) {
		Properties props = System.getProperties();
		props.put("mail.smtp.host", ConfigUtil.getConfig("mail.smtp"));
		props.put("mail.smtp.port", ConfigUtil.getConfig("mail.port"));
		// session
		javax.mail.Session session = javax.mail.Session
				.getInstance(props, null);
		Message msg = new javax.mail.internet.MimeMessage(session);
		try {
			msg.setFrom(new InternetAddress(ConfigUtil.getConfig("mail.from")));
			msg.setRecipients(Message.RecipientType.TO,
					InternetAddress.parse(ConfigUtil.getConfig("mail.to")));
			msg.setRecipients(Message.RecipientType.CC,
					InternetAddress.parse(ConfigUtil.getConfig("mail.cc")));
			msg.setSubject(ConfigUtil.getConfig("mail.subject"));
			// multipart
			BodyPart html = new MimeBodyPart();
			Iterator<String> iter = content.keySet().iterator();
			String template = getMailTemplate();
			while (iter.hasNext()) {
				String key = iter.next();
				template = template.replace("${" + key + "}", content.get(key));
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
		InputStream is = MailUtils.class.getClassLoader().getResourceAsStream(
				"mail.template");

		StringBuffer template = new StringBuffer();
		try {
			BufferedReader in = new BufferedReader(new InputStreamReader(is,
					"UTF-8"));
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

	public void test() {
		Map<String, String> map = new HashMap<String, String>();
		map.put("content", "test");
		sendMail(map);
	}

	public static boolean sendMail(String to, String subject, String str) {

		Properties props = System.getProperties();
		props.put("mail.smtp.host", ConfigUtil.getConfig("mail.smtp"));
		props.put("mail.smtp.port", ConfigUtil.getConfig("mail.port"));

		Session session = Session.getInstance(props, null);
		Message msg = new javax.mail.internet.MimeMessage(session);
		try {
			msg.setFrom(new InternetAddress(ConfigUtil.getConfig("mail.from")));
			msg.setRecipients(Message.RecipientType.TO,
					InternetAddress.parse(to, false));
			msg.setRecipients(Message.RecipientType.CC, InternetAddress.parse(
					ConfigUtil.getConfig("mail.cc"), false));
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
