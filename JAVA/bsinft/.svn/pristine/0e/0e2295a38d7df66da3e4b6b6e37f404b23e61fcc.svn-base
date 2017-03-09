package com.hola.common.mq;

import java.io.File;
import java.util.List;
import com.hola.common.exception.ReceiveMqException;
import com.hola.common.exception.SendMqException;

public interface IMqHelper 
{
	void sendFile(File file) throws SendMqException;
	List<File> receivFileForZip(File directory , String storeNo) throws ReceiveMqException;
	String getMqDetail();
}
