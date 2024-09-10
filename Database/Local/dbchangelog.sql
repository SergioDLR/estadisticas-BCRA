-- liquibase formatted sql

-- changeset serlopez:1

CREATE TABLE Variable (
    idVariable INT NOT NULL PRIMARY KEY,
    description varchar(255) NOT NULL,
    cdSerie int,
);


CREATE TABLE Value (
   	dateValue DATE ,
    idVariable INT,  
    value float NOT NULL,
    PRIMARY KEY(dateValue, idVariable),
    FOREIGN KEY (idVariable) REFERENCES Variable(idVariable),
);

-- rollback DROP TABLE Value;
-- rollback DROP TABLE Variable;