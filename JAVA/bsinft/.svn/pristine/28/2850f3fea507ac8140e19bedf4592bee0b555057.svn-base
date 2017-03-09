package com.hola.common.file;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Date;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import com.hola.common.ConfigHelper;
import com.hola.common.util.TimeUtil;

/**
 * 文件管理工具类
 * @author 王成(chengwangi@isoftstone.com)
 * @date 2013-1-6 下午3:37:52
 */
public class FileManager 
{
	private static Log log = LogFactory.getLog(FileManager.class);
	
	private	static File getDisk()
	{
		String prefix = ConfigHelper.getInstance().getValue(ConfigHelper.CSV_FILE_DIRECTORY); 
		if(prefix == null)
		{
			if(new File("d:\\").exists())
				prefix = "d:\\";
			if(new File("e:\\").exists())
				prefix = "e:\\";
			else if(new File("f:\\").exists())
				prefix = "f:\\";
		}
		log.info("csv保存目录!检测后将会保存在" + prefix);
		File file = new File(prefix);
		return file;
	}
	private	static File getRootFolder()
	{
		File diskFile = getDisk();
		String rootFolderName = ConfigHelper.getInstance().getValue(ConfigHelper.FOLDER_NAME_FOR_ROOT);
		return createFolder(diskFile, rootFolderName);
	}
	
	private static File getDateFolder(File storeFolder)
	{
		String currentDateStr = TimeUtil.formatDate(new Date(), TimeUtil.DATE_FORMAT_yyyyMMdd);
		return createFolder(storeFolder, currentDateStr);
	}
	
	private static File createFolder(File parentFolder, String folderName) {
		File folder = new File(parentFolder , folderName);
		//如果不存在目录，创建
		if(folder.exists() == false)
		{
			folder.mkdirs();
		}
		return folder;
	}
	
	/**
	 * 获取csv的保存路径，文件夹的生成规则是每天一个文件夹
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-6 下午2:36:02
	 * @return
	 * @throws IOException 
	 */
	private static File getCsvFileDirectory(String storeNo , String appFolderName) throws IOException
	{
		//查看配置中是否制定路径，如果未制定则在d盘创建，如果机器没有d盘则创建在e盘
		File rootFolder = getRootFolder();
		File appFolder = createFolder(rootFolder, appFolderName);
		File storeFolder = createFolder(appFolder, storeNo);
		File folder = getDateFolder(storeFolder);
		return folder;
	}
	/**
	 * 用来保存从MQ接收到的文件，即将要执行如BS的文件
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-10 下午2:35:54
	 * @return
	 * @throws IOException
	 */
	public static File getCsvFileDirectoryForMqToBs(String storeNo) throws IOException
	{
		File folder = getCsvFileDirectory(storeNo , ConfigHelper.getInstance().getValue(ConfigHelper.FOLDER_NAME_FOR_MQ_CSV_BS));
		log.info("保存文件的路径是：" + folder.getPath());
		return folder;
	}
	/**
	 * 用来保存从BS读取的数据发送到MQ的文件
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-10 下午2:38:06
	 * @return
	 * @throws IOException
	 */
	public static File getCsvFileDirectoryForBsToMq(String storeNo) throws IOException
	{
		File folder = getCsvFileDirectory(storeNo , ConfigHelper.getInstance().getValue(ConfigHelper.FOLDER_NAME_FOR_BS_CSV_MQ));
		log.info("保存文件的路径是：" + folder.getPath());
		return folder;
	}
	
	public static void writeStringToFile(File file , String str)
	{
		writeByteToFile(file , str.getBytes());
	}
	
	
	public static void writeByteToFile(File file , byte [] b)
	{
		try {
			FileOutputStream fos = new FileOutputStream(file);
			fos.write(b);
			fos.flush();
			fos.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	public static byte [] getByteByFile(File file) throws IOException
	{
		FileInputStream fis = new FileInputStream(file);
		byte [] b = new byte [(int) file.length()];
		fis.read(b);
		fis.close();
		return b;
	}
	
	public static void main(String[] args) {
		try {
			getCsvFileDirectoryForBsToMq("111");
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
