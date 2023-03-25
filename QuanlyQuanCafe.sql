CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

--Food
--Table
--FoodCategory
--Account
--Bill
--BillInfo

CREATE TABLE TableFood
(
	ID INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(100) NOT NULL DEFAULT N'NoName',
	[Status] NVARCHAR(100) NOT NULL DEFAULT N'Trống'-- EMPTY OR NOT
)
GO

CREATE TABLE Account 
(
	UserName NVARCHAR(100) PRIMARY KEY,
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'NoName',
	[Password] NVARCHAR(100) NOT NULL DEFAULT 0,
	[Type] INT NOT NULL DEFAULT 0 -- 0:staff, 1:admin
)
GO

CREATE TABLE FoodCategory
(
	ID INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
)
GO

CREATE TABLE Food
(
	ID INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	IDCategory INT NOT NULL,
	Price FLOAT NOT NULL

	FOREIGN KEY (IDCategory) REFERENCES dbo.FoodCategory(ID)
)
GO

CREATE TABLE Bill
(
	ID INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	IDTable INT NOT NULL,
	[Status] INT NOT NULL, --1: đã thanh toán, 0: chưa thanh toán
	Discount INT NOT NULL DEFAULT 0,
	TotalPrice FLOAT NOT NULL DEFAULT 0

	FOREIGN KEY (IDTable) REFERENCES dbo.TableFood(ID)
)
GO

CREATE TABLE BillInfo
(
	ID INT IDENTITY PRIMARY KEY,
	IDBill INT NOT NULL,
	IDFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0

	FOREIGN KEY (IDBill) REFERENCES dbo.Bill(ID),
	FOREIGN KEY (IDFood) REFERENCES dbo.Food(ID)
)
GO

INSERT INTO dbo.Account
		(
			UserName,
			DisplayName,
			[Password],
			[Type]
		)
	VALUES (
			 N'NxTG',
			 N'Trường Giang',
			 N'1',
			 1
		   )

INSERT INTO dbo.Account
		(
			UserName,
			DisplayName,
			[Password],
			[Type]
		)
	VALUES (
			 N'staff',
			 N'staff',
			 N'1',
			 0
		   )

GO

CREATE PROC USP_GetAccountListByUserName
@userName nvarchar(100)
as
begin
	SELECT * FROM dbo.Account WHERE Username = @userName
end
go

CREATE PROC USP_Login
@userName nvarchar(100), @password nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND [Password] = @password
END
GO

DECLARE @i INT = 0
WHILE @i <=10
BEGIN
	INSERT dbo.TableFood ([Name]) VALUES (N'Bàn ' + CAST(@i AS nvarchar(100)))
	SET @i = @i + 1
END
GO

CREATE PROC USP_GetTableList
AS SELECT * FROM dbo.TableFood
GO

--thêm danh mục
INSERT dbo.FoodCategory ([Name]) VALUES (N'Cafe')
INSERT dbo.FoodCategory ([Name]) VALUES (N'Sữa chua')
INSERT dbo.FoodCategory ([Name]) VALUES (N'Sinh tố')
INSERT dbo.FoodCategory ([Name]) VALUES (N'Nước hoa quả')
INSERT dbo.FoodCategory ([Name]) VALUES (N'Trà')
INSERT dbo.FoodCategory ([Name]) VALUES (N'Khác')

--thêm food
INSERT dbo.Food ([Name], IDCategory, Price) VALUES (N'Cafe Nâu', 1, 25000)
INSERT dbo.Food ([Name], IDCategory, Price) VALUES (N'Cafe Đen', 1, 25000)
INSERT dbo.Food ([Name], IDCategory, Price) VALUES (N'Sữa chua Cafe', 2, 30000)
INSERT dbo.Food ([Name], IDCategory, Price) VALUES (N'Sữa chua Thạch', 2, 30000)
INSERT dbo.Food ([Name], IDCategory, Price) VALUES (N'Sữa chua Cacao', 2, 30000)

GO

CREATE PROC USP_InsertBill
@idTable INT
AS
BEGIN
	INSERT dbo.Bill (DateCheckIn, DateCheckOut, IDTable, [Status], Discount)
		VALUES (GETDATE(), NULL, @idTable, 0, 0)
END
GO

CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN
	DECLARE	@isExistBillInfo INT
	DECLARE @foodCount INT

	SELECT @isExistBillInfo = id, @foodCount = b.count
	FROM dbo.BillInfo b
	WHERE IDBill = @idBill AND IDFood = @idFood

	IF (@isExistBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF (@newCount > 0)
			UPDATE dbo.BillInfo SET count = @newCount WHERE IDFood = @idFood AND IDBill = @idBill
		ELSE
			DELETE dbo.BillInfo WHERE IDBill = @idBill AND IDFood = @idFood
	END
	ELSE 
	BEGIN
		IF (@count > 0)
		BEGIN
		INSERT dbo.BillInfo (IDBill, IDFood, [count])
			VALUES (@idBill, @idFood, @count)
		END
	END
END
GO

CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = idBill FROM inserted

	DECLARE @idTable INT

	SELECT @idTable = IDTable From dbo.Bill WHERE ID = @idBill AND Status = 0

	DECLARE @count INT
	SELECT @count = COUNT(*) FROM dbo.BillInfo WHERE IDBill = @idBill

	IF (@count > 0)
		UPDATE dbo.TableFood SET Status = N'Có người' WHERE ID = @idTable
	ELSE
		UPDATE dbo.TableFood SET Status = N'Trống' WHERE ID = @idTable
END
GO

CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @idBill INT

	SELECT @idBill = id FROM inserted

	DECLARE @idTable INT

	SELECT @idTable = IDTable From dbo.Bill WHERE ID = @idBill 

	DECLARE @count INT

	SELECT @count = COUNT(*) FROM dbo.Bill WHERE IDTable = @idTable AND Status = 0

	IF(@count = 0)
		UPDATE dbo.TableFood SET Status = N'Trống' WHERE id = @idTable
END
GO

CREATE PROC USP_SwitchTable
@idFirstTable INT, @idSecondTable INT
AS
BEGIN
	DECLARE @idFirstBill INT
	DECLARE @idSecondBill INT

	DECLARE @isFirstTableEmpty INT = 1
	DECLARE @isSecondTableEmpty INT = 1

	SELECT @idFirstBill = ID FROM Bill WHERE IDTable = @idFirstTable AND Status = 0
	SELECT @idSecondBill = ID FROM Bill WHERE IDTable = @idSecondTable AND Status = 0

	IF (@idFirstBill IS NULL)
	BEGIN
		INSERT dbo.Bill (DateCheckIn, DateCheckOut, IDTable, Status)
			VALUES (GETDATE(), NULL, @idFirstTable, 0)
		SELECT @idFirstBill = Max(id) FROM dbo.Bill WHERE IDTable = @idFirstTable AND Status = 0
	END

	SELECT @isFirstTableEmpty = COUNT(*) FROM dbo.BillInfo WHERE IDBill = @idFirstBill

	IF (@idSecondBill IS NULL)
	BEGIN
		INSERT dbo.Bill (DateCheckIn, DateCheckOut, IDTable, Status)
			VALUES (GETDATE(), NULL, @idSecondTable, 0)
		SELECT @idSecondBill = Max(id) FROM dbo.Bill WHERE IDTable = @idSecondTable AND Status = 0
	END

	SELECT @isSecondTableEmpty = COUNT(*) FROM dbo.BillInfo WHERE IDBill = @idSecondBill

	SELECT ID INTO IDBillInfoTable FROM dbo.BillInfo WHERE IDBill = @idSecondBill

	UPDATE dbo.BillInfo SET idBill = @idSecondBill WHERE IDBill = @idFirstBill

	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)

	DROP TABLE IDBillInfoTable

	IF (@isFirstTableEmpty = 0)
		UPDATE dbo.TableFood SET Status = N'Trống' WHERE id = @idSecondTable
	IF (@isSecondTableEmpty = 0)
		UPDATE dbo.TableFood SET Status = N'Trống' WHERE id = @idFirstTable
END
GO

CREATE PROC USP_GetListBillByDate
@chechIn date, @checkOut date
AS
BEGIN
	SELECT t.Name as [Tên bàn], b.totalPrice As [Tổng tiền], dateCheckIn AS [Ngày vào], dateCheckOut AS [Ngày ra], discount AS [Giảm giá] 
	FROM dbo.Bill b, dbo.TableFood t
	WHERE DateCheckIn >= @chechIn AND DateCheckOut <= @checkOut 
		AND b.Status = 1 
		AND t.ID = b.IDTable
END
GO

CREATE PROC USP_UpdateAccount
@username NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPass NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT = 0

	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE Username = @username AND password = @password

	IF (@isRightPass = 1)
	BEGIN
		IF (@newPass IS NULL OR @newPass = '')
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @username
		ELSE 
			UPDATE dbo.Account SET DisplayName = @displayName, Password = @newPass WHERE UserName = @username
	END
END
GO

CREATE TRIGGER UTG_DeleteBillInfo
ON dbo.BillInfo FOR DELETE
AS
BEGIN
	DECLARE @idBillInfo INT
	DECLARE @idBill INT
	SELECT @idBillInfo = id, @idBill = deleted.idBill FROM deleted

	DECLARE @idTable INT
	SELECT @idTable = IDTable FROM dbo.Bill WHERE id = @idBill

	DECLARE @count INT = 0 
	SELECT @count = COUNT(*) FROM dbo.BillInfo bi, dbo.Bill b WHERE b.id = bi.idBill AND b.id = @idBill AND b.status = 0

	IF (@count = 0)
		UPDATE dbo.TableFood SET Status = N'Trống' WHERE id = @idTable
END
GO
