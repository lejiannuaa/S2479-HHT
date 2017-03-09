package com.hola.common.util;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Enumeration;
import java.util.List;
import java.util.zip.ZipEntry;
import java.util.zip.ZipFile;
import java.util.zip.ZipOutputStream;

/**
 * Java版的Zip工具
 * 
 */
public class ZipUtils {

	private static final int BUFF_SIZE = 1024 * 1024; // 1M Byte

	

	/**
	 * 解压缩一个文件
	 * 
	 * @param zipFile
	 *            压缩文件
	 * @param folderPath
	 *            解压缩的目标目录
	 * @throws IOException
	 *             当压缩过程出错时抛出
	 */
	@SuppressWarnings("rawtypes")
	public List<String> unZipFile(File zipFile, String folderPath) throws IOException {
		ZipFile zf = new ZipFile(zipFile);
		List<String> unzipFileName = new ArrayList<String>();
		for (Enumeration entries = zf.entries(); entries.hasMoreElements();) {
			ZipEntry entry = ((ZipEntry) entries.nextElement());

			InputStream in = zf.getInputStream(entry);
			OutputStream out = new FileOutputStream(folderPath + File.separator + entry.getName());
			byte buffer[] = new byte[BUFF_SIZE];
			int realLength;
			while ((realLength = in.read(buffer)) > 0) {
				out.write(buffer, 0, realLength);
			}
			in.close();
			out.close();
			unzipFileName.add(entry.getName());
		}
		return unzipFileName;
	}
	/**
	 * 解压缩一个文件
	 * 
	 * @param zipFile
	 *            压缩文件
	 * @param folderPath
	 *            解压缩的目标目录
	 * @throws IOException
	 *             当压缩过程出错时抛出
	 */
	@SuppressWarnings("rawtypes")
	public String unZipFileOne(File zipFile, String folderPath) throws IOException {
		ZipFile zf = new ZipFile(zipFile);
		for (Enumeration entries = zf.entries(); entries.hasMoreElements();) {
			ZipEntry entry = ((ZipEntry) entries.nextElement());

			InputStream in = zf.getInputStream(entry);
			OutputStream out = new FileOutputStream(folderPath + File.separator + entry.getName());
			byte buffer[] = new byte[BUFF_SIZE];
			int realLength;
			while ((realLength = in.read(buffer)) > 0) {
				out.write(buffer, 0, realLength);
			}
			in.close();
			out.close();
			return entry.getName();
		}
		return null;
	}
	
	public static void main(String[] args) 
	{
		File f = new File("d:\\public\\JSKUT4ALLS_20130258193302009.zip");
		File ff = null;
		try {
			String s = new ZipUtils().unZipFileOne(f, "d:\\public");
//			if(1 == 1)
//				return;
			ff = new File("d:\\public\\" + s);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		FileReader r = null;
		try {
			r = new FileReader(ff);
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		BufferedReader br = new BufferedReader(r);
		while(true)
		{
			String s = null;
			try {
				s = br.readLine();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			if(s != null)
				System.out.println(s);
			else
				break;
		}
		
//		
//		FileInputStream fis = new FileInputStream(ff);
//		byte [] b = new byte
//		fis.read(b);
	}

	private void zipFile(File resFile, ZipOutputStream zipout, String rootpath) throws IOException {
		rootpath = rootpath + (rootpath.trim().length() == 0 ? "" : File.separator) + resFile.getName();
		if (resFile.isDirectory()) {
			File[] fileList = resFile.listFiles();
			for (File file : fileList) {
				zipFile(file, zipout, rootpath);
			}
		} else {
			byte buffer[] = new byte[BUFF_SIZE];
			BufferedInputStream in = null;
			try{
				in = new BufferedInputStream(new FileInputStream(resFile), BUFF_SIZE);
//				Out.println(rootpath);
				zipout.putNextEntry(new ZipEntry(rootpath));
				int realLength;
				while ((realLength = in.read(buffer)) != -1) {
					zipout.write(buffer, 0, realLength);
				}
			}finally{
				if(in!=null)
					in.close();
				if(zipout!=null){
					zipout.flush();
					zipout.closeEntry();
				}
			}
		}
	}

	/**
	 * 批量压缩文件（夹）
	 * 
	 * @param resFileList
	 *            要压缩的文件（夹）列表
	 * @param zipFile
	 *            生成的压缩文件
	 * @throws IOException
	 *             当压缩过程出错时抛出
	 */
	public void zipFiles(Collection<File> resFileList, File zipFile) throws IOException {
		ZipOutputStream zipout = null;
		try {
			zipout = new ZipOutputStream(new BufferedOutputStream(new FileOutputStream(zipFile), BUFF_SIZE));
			for (File resFile : resFileList) {
				zipFile(resFile, zipout, "");
			}
		} finally {
			if (zipout != null) {
				zipout.flush();
				zipout.close();

			}
		}

	}
	
	/**
	 * 压缩一个文件
	 * 
	 * @param resFileList
	 *            要压缩的文件（夹）列表
	 * @param zipFile
	 *            生成的压缩文件
	 * @throws IOException
	 *             当压缩过程出错时抛出
	 */
	public void zipFile(File resFile, File zipFile) throws IOException {
		ZipOutputStream zipout = null;
		try {
			zipout = new ZipOutputStream(new BufferedOutputStream(new FileOutputStream(zipFile), BUFF_SIZE));
			zipFile(resFile, zipout, "");
		} finally {
			if (zipout != null) {
				zipout.flush();
				zipout.close();

			}
		}
	}

	/**
	 * 批量压缩文件（夹）
	 * 
	 * @param resFileList
	 *            要压缩的文件（夹）列表
	 * @param zipFile
	 *            生成的压缩文件
	 * @param comment
	 *            压缩文件的注释
	 * @throws IOException
	 *             当压缩过程出错时抛出
	 */
	public void zipFiles(Collection<File> resFileList, File zipFile, String comment) throws IOException {
		ZipOutputStream zipout = new ZipOutputStream(new BufferedOutputStream(new FileOutputStream(zipFile), BUFF_SIZE));
		for (File resFile : resFileList) {
			zipFile(resFile, zipout, "");
		}
		zipout.setComment(comment);
		zipout.close();
	}
	

}
