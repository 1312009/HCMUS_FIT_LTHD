CREATE DATABASE FOOD
GO
USE FOOD
GO
-- TẠO BẢNG DANH SÁCH NGƯỜI DÙNG
CREATE TABLE ACCOUNT
(
ID INT IDENTITY(1,1) PRIMARY KEY,
NAME NVARCHAR(100),
EMAIL NVARCHAR(100),
PASSWORDHASH NVARCHAR(200),
SALT NVARCHAR (200),
IMAGEACC NVARCHAR(100)
)
-- TẠO BẢNG DANH SÁCH ROLE CHO USER
CREATE TABLE ACCOUNT_ROLE
(
ID INT IDENTITY(1,1) PRIMARY KEY,
IDROLE INT,
IDUSER INT
)
-- TẠO BẢNG DANH SÁCH CÁC ROLE
CREATE TABLE LIST_ROLE
(
ID INT IDENTITY(1,1) PRIMARY KEY,
NAME NVARCHAR (100)
)
--THÊM BẢNG ĐĂNG NHẬP VỚI FACEBOKK,GOOGLE,...
CREATE TABLE EXTERNALACCOUNT
(
IDUSER INT NOT NULL ,
LOGINPROVIDER char (100),
PROVIDERKEY NVARCHAR(200) NOT NULL
)
--THÊM KHÓA CHÍNH CHO BẢNG EXTERNALACCOUNT
ALTER TABLE EXTERNALACCOUNT
ADD CONSTRAINT PK_EXTERNALACCOUNT
PRIMARY KEY(IDUSER,PROVIDERKEY) 

--THÊM BẢNG MÓN ĂN
CREATE TABLE FOOD
(
ID INT IDENTITY(1,1) PRIMARY KEY,
NAME NVARCHAR (100),
DECRIPTION NVARCHAR(500),
IDTYPE INT,
STATUSFOOD INT,
IMGFOOD NVARCHAR(200),
PRICE FLOAT,
ISSALE INT
)
--THÊM BẢNG LIKE
CREATE TABLE ORDERFOOD
(
ID INT IDENTITY(1,1) PRIMARY KEY,
IDUSER INT NOT NULL,
IDFOOD INT NOT NULL 
)

--TẠO RÀNG BUỘC CHO BẢNG FOOD
ALTER TABLE FOOD
ADD CONSTRAINT C_FOOD
CHECK ( STATUSFOOD IN(1,0))
--THÊM BẢNG LOẠI THỨC ĂN
CREATE TABLE TYPEFOOD
(
ID INT IDENTITY(1,1) PRIMARY KEY,
NAME NVARCHAR (20),
TIMESTART TIME,
TIMEEND TIME
)

--THÊM DỮ LIỆU BẢNG ACCOUNT
INSERT INTO LIST_ROLE(NAME) VALUES ('ADMIN')
INSERT INTO LIST_ROLE(NAME) VALUES ('CUSTOMER')
-- THÊM DỮ LIỆU BẢNG TYPE_FOOD
INSERT INTO TYPEFOOD(NAME,TIMESTART,TIMEEND) VALUES (N'ĐIỂM TÂM','06:00','9:00')
INSERT INTO TYPEFOOD(NAME,TIMESTART,TIMEEND) VALUES (N'CƠM TRƯA','10:00','13:00')
INSERT INTO TYPEFOOD(NAME,TIMESTART,TIMEEND) VALUES (N'CƠM TỐI','16:00','21:00')
--THÊM DỮ LIỆU BẢNG FOOD
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Hàu vọng nguyệt',N'Là món ăn vừa ngon, vừa bổ,
 với cách trình bày lạ mắt, hơn nữa hàu có hàm lượng chất béo thấp, chất kẽm cao, ăn vào
 sẽ đẹp da, giữ dáng.','http://res.cloudinary.com/dqabuxewl/image/upload/v1480927282/5484_nj6ph4.jpg',2,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Gà nướng bưởi xốt giấm',N'Gà nướng bưởi xốt giấm 
là món ăn ngon, giàu vitamin. Ngoài ra bưởi có tác dụng bổ dưỡng cơ thể, phòng và chữa một số bệnh như cao huyết áp, tiểu đường.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480927281/5472_s96ntq.jpg',2,50000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Bún giò heo nấu giấm',N'Là món ăn lạ miệng và rất được ưa 
thích ở miền Tây Nam bộ, giúp cân bằng lại sau những ngày bồi bổ đầy đủ dưỡng chất, đảm bảo ăn không ngán ',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480927604/5017_tdodok.jpg',1,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Bún ốc',N'Bún ốc là một món ngon cân bằng dưỡng chất,
 hấp dẫn, thích hợp nhất là vừa ăn Tết xong, mọi người cứ no ngang, hậu quả của việc ăn quá nhiều thịt.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480928085/5016_xh1dzj.jpg',1,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Lẩu chua thịt bò',N'Lẩu chua thịt bò là món ăn ngon, 
rất hấp dẫn, trong nồi lẩu sẽ có sắc vàng như màu hoa mai, sắc xanh như màu lộc biếc, thích hợp với không khí ngày Tết.'
,'http://res.cloudinary.com/dqabuxewl/image/upload/v1480928390/5004_hhltem.jpg',3,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Súp nghêu rau củ',N'Món ăn này có xuất xứ từ món súp của các 
nước châu Âu, châu Mỹ nhưng nguyên liệu và gia vị lại rất phù hợp với ẩm thực Việt Nam, có thể dùng làm thay đổi khẩu vị bữa ăn gia đình.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480928504/5411_ebtssq.jpg',1,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Súp sắc màu',N'Một món ăn có nhiều sắc màu sẽ cũng cấp nhiều 
và đa dạng các chất dinh dưỡng, vitamin và khoáng chất, rất tốt cho sức khỏe.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480928614/5456_hedfq2.jpg',1,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Cá chiên hoa cúc',N'Đây là món ăn ngày Tết thật đơn
 giản và ít tốn thời gian chế biến, mang ý nghĩa trường thọ và giàu dinh dưỡng.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480928712/5448_yy2rj9.jpg',3,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Vịt kho nước tương',N'Vịt kho nước tương là món ăn 
vừa ngon vừa có tác dụng cân bằng hàn-nhiệt, mang lại lợi ích cho sức khỏe.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480928847/5394_rw90hh.jpg',3,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Chim cút nhồi cốm hấp',N'Là 1 món ăn ngon bổ dưỡng 
cho sức khỏe với các nguyên liệu','http://res.cloudinary.com/dqabuxewl/image/upload/v1480928954/5464_f3punz.jpg',3,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Nhộng rang lá chanh bắc',N'Mang hương vị đậm đà khi 
được chế biến với hương lá chanh nồng nàn, tạo sự kích thích về khứu giác và vị giác, món ăn này có nét rất riêng biệt, phù hợp với thời 
tiết se lạnh cuối năm.','http://res.cloudinary.com/dqabuxewl/image/upload/v1480929091/5096_rg4xb7.jpg',3,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Giò heo hầm bông atiso',N'Với tiêu chí món ăn ngon, 
bổ, dễ thực hiện của chương trình, món giò heo hầm bông atiso là một sự lựa chọn hoàn hảo cho gia đình vào những dịp cuối tuần, giúp 
nâng cao sức khỏe cho cả người lớn lẫn trẻ em.','http://res.cloudinary.com/dqabuxewl/image/upload/v1480929186/4964_goevvp.jpg',3,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Vịt xốt chua ngọt',N'Với vịt quay, vịt luộc hay vịt xào sả ớt... 
là những món quá quen thuộc với chúng ta rồi. Hôm nay chúng tôi giới thiệu một món ngon từ thịt vịt hoàn toàn mới là "vịt xốt chua ngọt" 
các bạn đã thử chưa?','http://res.cloudinary.com/dqabuxewl/image/upload/v1480929298/4770_gbtnc8.jpg',2,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Cơm chiên xốt tương',N'Món ăn từ Nhật nhưng 
lại rất gần gũi với khẩu vị người Việt, mang hương vị độc đáo, thơm ngon',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480929470/5426_syhv7k.jpg',2,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Pha Chế Magarita Dưa Mix Bơ Lạ Miệng',N'Lại có thêm 
món đồ uống đặc biệt thơm ngon nữa rồi này.','http://res.cloudinary.com/dqabuxewl/image/upload/v1480929584/4728_ma7muc.jpg',2,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Cật heo xào bí ngòi',N'Đây là món ăn ngon, với cật heo bổ
 dưỡng cho thận có tác dụng bồi bổ cơ thể','http://res.cloudinary.com/dqabuxewl/image/upload/v1480929878/5479_hfucxh.jpg',2,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Giò thủ xào',N'Món giò thủ là món ăn truyền thống rất 
phổ biến của Việt Nam trong những ngày lễ Tết.','http://res.cloudinary.com/dqabuxewl/image/upload/v1480929988/5390_oa17ux.jpg',2,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'BÚN BÒ HUẾ',N'Thịt bò xào lá lốt cũng là một món ngon 
không kém món Bò nướng lá lốt. Miếng bò chín mềm vẫn giữ được vị ngọt, lá lốt thơm mùi đặc trưng. Ngoài ra còn có vị beo béo ngọt ngọt 
của kẹo đậu phộng. Món ăn có nguyên liệu đơn giản nhưng rất hấp dẫn đúng không các bạn!',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480930059/5359_tq3lvr.jpg',2,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Tôm xào sả cay',N'Tôm xào sả cay là một món ăn ngon,
 sử dụng nguyên liệu sả có tác dụng chữa được nhiều bệnh như đầy bụng, cảm cúm.',
 'http://res.cloudinary.com/dqabuxewl/image/upload/v1480930139/5400_guk008.jpg',2,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Lươn xào lăn',N'Lươn xào lăn là một món ăn rất 
bổ và trị được nhiều bệnh ngon và được rất nhiều người yêu thích.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480930231/5403_siwlaq.jpg',3,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Tôm rang ruốc tôm',N'Tôm rang ruốc tôm là món ngon, 
hấp dẫn phù hợp trong thời tiết se lạnh của những ngày đầu đông này. Món ăn được chế biến khá đơn giản với những nguyên liệu thường 
dùng hàng ngày nhưng thành phẩm lại làm cho bạn hương vị thơm ngon, lạ miệng đến khó quên.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480930337/4779_aetkir.jpg',3,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Bánh tét lá cẩm',N'Bánh tét lá cẩm là món bánh truyền thống 
của người Việt mình nhân dịp Tết Nguyên Đán, ngoài ra còn là đặc sản của nhiều vùng Nam Bộ nữa.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480931111/5375_onrrvl.jpg',1,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Bánh há cảo hoa hồng',N'Là món bánh quen thuộc 
nhưng được trình bày lạ, đẹp mắt, mang hương vị thơm ngon, hấp dẫn.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480931242/5431_iv5zhp.jpg',1,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Gỏi trứng chiên','Gỏi trứng chiên có nguồn gốc 
từ Thái Lan có vị chua dịu ngọt và hơi hơi cay, màu sắc bắt mắt thích hợp để làm mới bữa ăn cuối tuần.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480931342/5459_jvhb6v.jpg',1,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Gỏi gà rau răm',N'Món gỏi có vị rất đậm đà 
vừa ăn, chỉ hơi chua nhẹ dễ chịu, có sự cộng hưởng thú vị giữa gà và rau răm.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480931428/5407_jkjokx.jpg',3,30000,1,1)
INSERT INTO FOOD(NAME,DECRIPTION,IMGFOOD,IDTYPE,PRICE,ISSALE,STATUSFOOD) VALUES (N'Nộm hoa chuối hải sản',
N'Đây là món ăn dân dã rất được ưa thích, là một trong năm loại gỏi của món gỏi ngũ sắc, khá lạ miệng và dễ thực hiện.',
'http://res.cloudinary.com/dqabuxewl/image/upload/v1480931534/5406_xw0gkv.jpg',2,30000,1,1)
go
CREATE PROCEDURE usp_TopMonDatHang
AS
BEGIN
	SELECT F.ID,F.NAME,F.DECRIPTION,F.ISSALE,F.IDTYPE,F.IMGFOOD,F.PRICE,F.STATUSFOOD,COUNT(*) AS COUNT
	FROM FOOD F JOIN ORDERFOOD L ON F.ID=L.IDFOOD
	GROUP BY F.ID,F.NAME,F.DECRIPTION,F.ISSALE,F.IDTYPE,F.IMGFOOD,F.PRICE,F.STATUSFOOD
	ORDER BY COUNT(*) DESC
END
GO
create procedure usp_TimKiemMonAn @TT nvarchar(50),@row int,@count int
as
begin
	SET FMTONLY OFF
	if(@count is null and @row is null )
	begin
		set @row=0
		select @count = count(*)
		from(select distinct ID from FOOD where  dbo.fChuyenCoDauThanhKhongDau(NAME) like N'%'+@TT+'%') a
	end
	CREATE TABLE #TMP (MAMON INT, SOLUONGDATHANG INT)
	INSERT INTO #TMP
	SELECT F.ID AS MAMON, "SOLUONGDATHANG" = 
				CASE
					WHEN SOLUONGDATHANG IS NULL THEN 0
					ELSE SOLUONGDATHANG
				END
	FROM FOOD F LEFT JOIN (SELECT IDFOOD  AS MAMON, COUNT(*) AS SOLUONGDATHANG
								FROM LIKEUSER
								GROUP BY IDFOOD) TH ON F.ID = TH.MAMON
	ORDER BY SOLUONGDATHANG DESC
	
	SELECT F.ID,F.NAME,F.DECRIPTION,F.ISSALE,F.IDTYPE,F.IMGFOOD,F.PRICE,F.STATUSFOOD,
		#TMP.SOLUONGDATHANG AS SOLUONGDATHANG
	from FOOD F, #TMP,
				(select ID,row_number() over(order by count(*) desc) as row
				from FOOD where  dbo.fChuyenCoDauThanhKhongDau(NAME) like N'%'+@TT+'%'
				group by ID) m1
	where m1.row>@row and m1.row <=@count+@row and F.ID= m1.ID
	AND #TMP.MAMON = F.ID
	order by #TMP.SOLUONGDATHANG desc
	--DROP TABLE #TMP
end
go 
CREATE FUNCTION [dbo].[fChuyenCoDauThanhKhongDau](@inputVar NVARCHAR(MAX) )
RETURNS NVARCHAR(MAX)
AS
BEGIN    
    IF (@inputVar IS NULL OR @inputVar = '')  RETURN ''
   
    DECLARE @RT NVARCHAR(MAX)
    DECLARE @SIGN_CHARS NCHAR(256)
    DECLARE @UNSIGN_CHARS NCHAR (256)
 
    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' + NCHAR(272) + NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
 
    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
   
    SET @COUNTER = 1
    WHILE (@COUNTER <= LEN(@inputVar))
    BEGIN  
        SET @COUNTER1 = 1
        WHILE (@COUNTER1 <= LEN(@SIGN_CHARS) + 1)
        BEGIN
            IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@inputVar,@COUNTER ,1))
            BEGIN          
                IF @COUNTER = 1
                    SET @inputVar = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@inputVar, @COUNTER+1,LEN(@inputVar)-1)      
                ELSE
                    SET @inputVar = SUBSTRING(@inputVar, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@inputVar, @COUNTER+1,LEN(@inputVar)- @COUNTER)
                BREAK
            END
            SET @COUNTER1 = @COUNTER1 +1
        END
        SET @COUNTER = @COUNTER +1
    END
    -- SET @inputVar = replace(@inputVar,' ','-')
    RETURN @inputVar
END
