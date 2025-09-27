-- Write your own SQL object definition here, and it'll be included in your package.
-- =============================================
-- BORRAR Y CREAR BASE DE DATOS
-- =============================================
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'InventarioPlus')
BEGIN
    ALTER DATABASE InventarioPlus SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE InventarioPlus;
END
GO

CREATE DATABASE InventarioPlus;
GO

USE InventarioPlus;
GO

-- =============================================
-- TABLA DE USUARIOS
-- =============================================
CREATE TABLE Usuarios (
                          UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
                          Nombre NVARCHAR(100) NOT NULL,
                          NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
                          Contrasena NVARCHAR(255) NOT NULL,
                          Rol NVARCHAR(20) NOT NULL CHECK (Rol IN ('Empleado','Administrador'))
);
GO

-- =============================================
-- TABLA DE PROVEEDORES
-- =============================================
CREATE TABLE Proveedores (
                             ProveedorID INT IDENTITY(1,1) PRIMARY KEY,
                             Nombre NVARCHAR(100) NOT NULL,
                             Telefono NVARCHAR(20),
                             Correo NVARCHAR(100),
                             Direccion NVARCHAR(255),
                             Categoria NVARCHAR(100)
);
GO

-- =============================================
-- TABLA DE PRODUCTOS
-- =============================================
CREATE TABLE Productos (
                           ProductoID INT IDENTITY(1,1) PRIMARY KEY,
                           Codigo NVARCHAR(50) NOT NULL UNIQUE,
                           Nombre NVARCHAR(100) NOT NULL,
                           Descripcion NVARCHAR(255),
                           Stock INT NOT NULL DEFAULT 0,
                           StockApartado INT NOT NULL DEFAULT 0,
                           StockMinimo INT NOT NULL DEFAULT 0
);
GO

-- =============================================
-- TABLA DE ATRIBUTOS DINÁMICOS DE PRODUCTOS
-- =============================================
CREATE TABLE ProductoAtributos (
                                   ProductoAtributoID INT IDENTITY(1,1) PRIMARY KEY,
                                   ProductoID INT NOT NULL,
                                   NombreAtributo NVARCHAR(100) NOT NULL,
                                   Valor NVARCHAR(255) NOT NULL,
                                   FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);
GO

-- =============================================
-- TABLA DE MOVIMIENTOS
-- =============================================
CREATE TABLE Movimientos (
                             MovimientoID INT IDENTITY(1,1) PRIMARY KEY,
                             FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
                             UsuarioID INT NOT NULL,
                             ProductoID INT NOT NULL,
                             TipoMovimiento NVARCHAR(10) NOT NULL CHECK (TipoMovimiento IN ('Entrada','Salida')),
                             Cantidad INT NOT NULL CHECK (Cantidad > 0),
                             FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
                             FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);
GO

-- =============================================
-- TABLA DE RESERVAS
-- =============================================
CREATE TABLE Reservas (
                          ReservaID INT IDENTITY(1,1) PRIMARY KEY,
                          NumeroReserva NVARCHAR(50) NOT NULL UNIQUE,
                          ProductoID INT NOT NULL,
                          CantidadReservada INT NOT NULL CHECK (CantidadReservada > 0),
                          FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
                          FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);
GO

-- =============================================
-- TRIGGER PARA ACTUALIZAR STOCK EN MOVIMIENTOS
-- =============================================
CREATE TRIGGER trg_ActualizarStockMovimientos
    ON Movimientos
    AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

UPDATE p
SET p.Stock =
        CASE
            WHEN i.TipoMovimiento = 'Entrada' THEN p.Stock + i.Cantidad
            WHEN i.TipoMovimiento = 'Salida' THEN p.Stock - i.Cantidad
            END
    FROM Productos p
    INNER JOIN inserted i ON p.ProductoID = i.ProductoID;
END;
GO

-- =============================================
-- TRIGGER PARA ACTUALIZAR STOCK EN RESERVAS
-- =============================================
CREATE TRIGGER trg_ActualizarStockReservas
    ON Reservas
    AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

UPDATE p
SET p.StockApartado = p.StockApartado + i.CantidadReservada,
    p.Stock = p.Stock - i.CantidadReservada
    FROM Productos p
    INNER JOIN inserted i ON p.ProductoID = i.ProductoID;
END;
GO
