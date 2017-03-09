package com.hola.jda2hht.util;

import java.io.File;
import java.io.RandomAccessFile;
import java.nio.channels.FileChannel;

public class FileUtil {

	private FileUtil() {

	}

	/**
	 * 复制文件
	 * 
	 * @file: FileUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-5
	 * @param srcPath
	 * @param targetPath
	 * @throws Exception
	 */
	public static void copyFile(String srcPath, String targetPath)
			throws Exception {
		RandomAccessFile srcrf = new RandomAccessFile(new File(srcPath), "r");
		RandomAccessFile tagrf = new RandomAccessFile(new File(targetPath),
				"rws");
		try {
			FileChannel srcFileChannel = srcrf.getChannel();
			FileChannel tagFileChannel = tagrf.getChannel();
			long size = srcrf.length();
			// 这里是可以改进的，比如，一次复制1024字节，多次复制，
			// 改变position和size就可以，我是没空搞这个了
			tagFileChannel.transferFrom(srcFileChannel, 0, size);
		} finally {
			// 关闭文件句柄
			srcrf.close();
			tagrf.close();
		}
	}

}
