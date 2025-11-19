------------------------------------------------------------
-- PROC: Listar usuários
------------------------------------------------------------
IF OBJECT_ID('dbo.usp_Usuarios_Listar', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Usuarios_Listar;
GO

CREATE PROCEDURE dbo.usp_Usuarios_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        Nome,
        Cpf,
        Telefone
    FROM dbo.Usuarios
    ORDER BY Id;
END;
GO

------------------------------------------------------------
-- PROC: Obter usuário por Id
------------------------------------------------------------
IF OBJECT_ID('dbo.usp_Usuarios_Obter', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Usuarios_Obter;
GO

CREATE PROCEDURE dbo.usp_Usuarios_Obter
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        Nome,
        Cpf,
        Telefone
    FROM dbo.Usuarios
    WHERE Id = @Id;
END;
GO

------------------------------------------------------------
-- PROC: Adicionar usuário
-- Retorna o Id gerado via SELECT SCOPE_IDENTITY()
------------------------------------------------------------
IF OBJECT_ID('dbo.usp_Usuarios_Adicionar', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Usuarios_Adicionar;
GO

CREATE PROCEDURE dbo.usp_Usuarios_Adicionar
    @Nome     NVARCHAR(100),
    @Cpf      CHAR(11),
    @Telefone VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Usuarios (Nome, Cpf, Telefone)
    VALUES (@Nome, @Cpf, @Telefone);

    -- mantém compatibilidade com ExecuteScalar() do repositório
    SELECT SCOPE_IDENTITY() AS IdGerado;
END;
GO

------------------------------------------------------------
-- PROC: Atualizar usuário
------------------------------------------------------------
IF OBJECT_ID('dbo.usp_Usuarios_Atualizar', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Usuarios_Atualizar;
GO

CREATE PROCEDURE dbo.usp_Usuarios_Atualizar
    @Id       INT,
    @Nome     NVARCHAR(100),
    @Cpf      CHAR(11),
    @Telefone VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Usuarios
       SET Nome     = @Nome,
           Cpf      = @Cpf,
           Telefone = @Telefone
     WHERE Id       = @Id;
END;
GO

------------------------------------------------------------
-- PROC: Excluir usuário
------------------------------------------------------------
IF OBJECT_ID('dbo.usp_Usuarios_Excluir', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Usuarios_Excluir;
GO

CREATE PROCEDURE dbo.usp_Usuarios_Excluir
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Usuarios
    WHERE Id = @Id;
END;
GO
