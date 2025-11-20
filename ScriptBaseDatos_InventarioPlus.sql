-- =============================================
-- Script de Base de Datos: InventarioPlus
-- Creado para SQL Server
-- =============================================

USE master;
GO

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'InventarioPlus')
BEGIN
    CREATE DATABASE InventarioPlus;
END
GO

USE InventarioPlus;
GO

-- =============================================
-- Tabla: Usuarios
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
        Contrasena NVARCHAR(255) NOT NULL,
        Rol NVARCHAR(50) NOT NULL
    );
END
GO

-- =============================================
-- Tabla: Categorias
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categorias')
BEGIN
    CREATE TABLE Categorias (
        IDCategoria INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL
    );
END
GO

-- =============================================
-- Tabla: Productos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Productos')
BEGIN
    CREATE TABLE Productos (
        ProductoID INT IDENTITY(1,1) PRIMARY KEY,
        Codigo NVARCHAR(50) NOT NULL UNIQUE,
        Nombre NVARCHAR(150) NOT NULL,
        Descripcion NVARCHAR(500) NULL,
        Stock INT NOT NULL DEFAULT 0,
        StockApartado INT NOT NULL DEFAULT 0,
        StockMinimo INT NOT NULL DEFAULT 0,
        IDCategoria INT NULL,
        PrecioCompra DECIMAL(18,2) NOT NULL DEFAULT 0,
        PrecioVenta DECIMAL(18,2) NOT NULL DEFAULT 0,
        CONSTRAINT FK_Productos_Categorias FOREIGN KEY (IDCategoria) 
            REFERENCES Categorias(IDCategoria)
    );
END
GO

-- =============================================
-- Tabla: Proveedores
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Proveedores')
BEGIN
    CREATE TABLE Proveedores (
        ProveedorID INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(150) NOT NULL,
        Telefono NVARCHAR(20) NULL,
        Correo NVARCHAR(100) NULL,
        Direccion NVARCHAR(255) NULL,
        Categoria NVARCHAR(100) NULL
    );
END
GO

-- =============================================
-- Tabla: ProductoProveedores (Relación Muchos a Muchos)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProductoProveedores')
BEGIN
    CREATE TABLE ProductoProveedores (
        ProductoID INT NOT NULL,
        ProveedorID INT NOT NULL,
        PRIMARY KEY (ProductoID, ProveedorID),
        CONSTRAINT FK_ProductoProveedores_Productos FOREIGN KEY (ProductoID) 
            REFERENCES Productos(ProductoID) ON DELETE CASCADE,
        CONSTRAINT FK_ProductoProveedores_Proveedores FOREIGN KEY (ProveedorID) 
            REFERENCES Proveedores(ProveedorID) ON DELETE CASCADE
    );
END
GO

-- =============================================
-- Tabla: Movimientos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Movimientos')
BEGIN
    CREATE TABLE Movimientos (
        MovimientoID INT IDENTITY(1,1) PRIMARY KEY,
        FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
        UsuarioID INT NOT NULL,
        ProductoID INT NOT NULL,
        TipoMovimiento NVARCHAR(50) NOT NULL, -- 'Entrada' o 'Salida'
        Cantidad INT NOT NULL,
        CONSTRAINT FK_Movimientos_Usuarios FOREIGN KEY (UsuarioID) 
            REFERENCES Usuarios(UsuarioID),
        CONSTRAINT FK_Movimientos_Productos FOREIGN KEY (ProductoID) 
            REFERENCES Productos(ProductoID)
    );
END
GO

-- =============================================
-- Tabla: ProductoAtributos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProductoAtributos')
BEGIN
    CREATE TABLE ProductoAtributos (
        ProductoAtributoID INT IDENTITY(1,1) PRIMARY KEY,
        ProductoID INT NOT NULL,
        NombreAtributo NVARCHAR(100) NOT NULL,
        Valor NVARCHAR(255) NOT NULL,
        CONSTRAINT FK_ProductoAtributos_Productos FOREIGN KEY (ProductoID) 
            REFERENCES Productos(ProductoID) ON DELETE CASCADE
    );
END
GO

-- =============================================
-- Tabla: Reservas
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reservas')
BEGIN
    CREATE TABLE Reservas (
        ReservaID INT IDENTITY(1,1) PRIMARY KEY,
        NumeroReserva NVARCHAR(50) NOT NULL UNIQUE,
        ProductoID INT NOT NULL,
        CantidadReservada INT NOT NULL,
        FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
        Cliente NVARCHAR(100) NULL,
        CONSTRAINT FK_Reservas_Productos FOREIGN KEY (ProductoID) 
            REFERENCES Productos(ProductoID)
    );
END
GO

-- =============================================
-- Datos Iniciales: Usuarios
-- =============================================
IF NOT EXISTS (SELECT * FROM Usuarios WHERE NombreUsuario = 'admin')
BEGIN
    INSERT INTO Usuarios (Nombre, NombreUsuario, Contrasena, Rol)
    VALUES 
        ('Administrador General', 'admin', 'admin123', 'Administrador'),
        ('Empleado Juan', 'juan', 'juan123', 'Empleado');
END
GO

-- =============================================
-- Datos Iniciales: Categorías
-- =============================================
IF NOT EXISTS (SELECT * FROM Categorias)
BEGIN
    INSERT INTO Categorias (Nombre)
    VALUES 
        ('Alimentos y Bebidas'),
        ('Electrónica'),
        ('Ropa y Accesorios'),
        ('Hogar y Jardín'),
        ('Deportes y Recreación'),
        ('Salud y Belleza'),
        ('Oficina y Papelería'),
        ('Juguetes y Juegos');
END
GO

-- =============================================
-- Índices para mejorar rendimiento
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Productos_Codigo')
BEGIN
    CREATE INDEX IX_Productos_Codigo ON Productos(Codigo);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Productos_IDCategoria')
BEGIN
    CREATE INDEX IX_Productos_IDCategoria ON Productos(IDCategoria);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Movimientos_FechaHora')
BEGIN
    CREATE INDEX IX_Movimientos_FechaHora ON Movimientos(FechaHora);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Movimientos_ProductoID')
BEGIN
    CREATE INDEX IX_Movimientos_ProductoID ON Movimientos(ProductoID);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Reservas_NumeroReserva')
BEGIN
    CREATE INDEX IX_Reservas_NumeroReserva ON Reservas(NumeroReserva);
END
GO

-- =============================================
-- Fin del Script
-- =============================================
PRINT 'Base de datos InventarioPlus creada exitosamente.';
GO

