package com.hola.common.mq;


import javax.jms.BytesMessage;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.Session;
import org.springframework.jms.core.MessageCreator;


public class ByteMessageCreatorImpl implements MessageCreator 
{
	private		String			msgId;
	private		byte[]	 		bytes;
	
	
	public ByteMessageCreatorImpl(String msgId , byte[] msgBody) {
		this.msgId = msgId;
		this.bytes = msgBody;
	}


	@Override
	public Message createMessage(Session session) throws JMSException 
	{
		BytesMessage msg = session.createBytesMessage();
		msg.setStringProperty("MSG_ID", this.msgId);
		msg.writeBytes(bytes);
		return msg;
	}

}
