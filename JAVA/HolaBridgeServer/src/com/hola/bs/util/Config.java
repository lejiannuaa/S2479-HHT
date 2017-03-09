package com.hola.bs.util;

public class Config {
    private String type;
    private String direction;
    private String id;
    
    public Config() {
        super();
    }
    public Config(String type, String direction, String id) {
        super();
        this.type = type;
        this.direction = direction;
        this.id = id;
    }
    public String getType() {
        return type;
    }
    public String getDirection() {
        return direction;
    }
    public String getId() {
        return id;
    }
    public void setType(String type) {
        this.type = type;
    }
    public void setDirection(String direction) {
        this.direction = direction;
    }
    public void setId(String id) {
        this.id = id;
    }
    
    
}
