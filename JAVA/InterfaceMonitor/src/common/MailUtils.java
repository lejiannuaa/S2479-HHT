package common;

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

import common.ConfigHelper;

public class MailUtils {
	
	
	public static boolean sendMail(Map<String, String> content, String subject) {
		Properties props = System.getProperties();
		props.put("mail.smtp.host", ConfigHelper.getInstance().getValue("mail.smtp"));
		props.put("mail.smtp.port", ConfigHelper.getInstance().getValue("mail.port"));
		// session
		javax.mail.Session session = javax.mail.Session
				.getInstance(props, null);
		Message msg = new javax.mail.internet.MimeMessage(session);
		try {
			msg.setFrom(new InternetAddress(ConfigHelper.getInstance().getValue("mail.from")));
			msg.setRecipients(Message.RecipientType.TO,
					InternetAddress.parse(ConfigHelper.getInstance().getValue("mail.to")));
			msg.setRecipients(Message.RecipientType.CC,
					InternetAddress.parse(ConfigHelper.getInstance().getValue("mail.cc")));
			msg.setSubject(subject);
			//msg.setSubject(ConfigHelper.getInstance().getValue("mail.subject"));
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
			return true;
		} catch (AddressException e) {
			e.printStackTrace();
		} catch (MessagingException e) {
			e.printStackTrace();
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
			e.printStackTrace();
		}
		return template.toString();
	}

	public void test() {
		Map<String, String> map = new HashMap<String, String>();
		map.put("content", "test");
		//sendMail(map);
	}

	public static boolean sendMail(String to, String subject, String str) {

		Properties props = System.getProperties();
		props.put("mail.smtp.host", ConfigHelper.getInstance().getValue("mail.smtp"));
		props.put("mail.smtp.port", ConfigHelper.getInstance().getValue("mail.port"));

		Session session = Session.getInstance(props, null);
		Message msg = new javax.mail.internet.MimeMessage(session);
		try {
			msg.setFrom(new InternetAddress(ConfigHelper.getInstance().getValue("mail.from")));
			msg.setRecipients(Message.RecipientType.TO,
					InternetAddress.parse(to, false));
			msg.setRecipients(Message.RecipientType.CC, InternetAddress.parse(
					ConfigHelper.getInstance().getValue("mail.cc"), false));
			msg.setSubject(subject);

			BodyPart html = new MimeBodyPart();
			html.setContent(str, "text/html;charset=utf8");

			Multipart multiPart = new MimeMultipart();
			multiPart.addBodyPart(html);

			msg.setContent(multiPart);
			msg.setSentDate(new Date());
			javax.mail.Transport.send(msg);
			return true;
		} catch (AddressException e) {
			e.printStackTrace();
		} catch (MessagingException e) {
			e.printStackTrace();
		}
		return false;
	}
	public static boolean sendMail(String content, String subject)
	{
		Map<String, String> contentMap = new HashMap<String, String>() ;
		contentMap.put("content", content);
		return MailUtils.sendMail(contentMap, subject);
	}

}
