use PolicyPro371;
Select * From Tbl_Policy;
Select * From Tbl_PolicyAttributes;
Select * From Tbl_Company;
Select * From Tbl_UserPolicy;
Select * From Tbl_UserPayment;
Select * From Tbl_Users;
Select * From Tbl_Admin;
Select * From Tbl_AdminWallet;
Select * From Tbl_CompanyWallet;
Select * From Tbl_TransactionHistory;
Select * From Tbl_UserClaims;
Select * From Tbl_UserWallet;
Select * From Tbl_Contact;
Select * From Tbl_faq;
INSERT INTO [PolicyPro371].[dbo].[Tbl_Admin] 
       ([Name], [Email], [Password], [Img])
VALUES 
       ('Admin', 'admin@gmail.com', '123456', NULL);

UPDATE [PolicyPro371].[dbo].[Tbl_UserPolicy]
SET [ExpiryDate] = '2025-10-03'
WHERE [Id] = 1;


UPDATE Tbl_LoanInstallments
SET DueDate = DATEADD(day, -11, GETDATE()), Status = 'Unpaid', PaidDate = NULL
WHERE Id = 1;

Select * From Tbl_LoanPayments;
Select * From Tbl_LoanInstallments;
Select * From Tbl_LoanRequests;
SELECT lr.Id AS LoanRequestId, lr.PolicyId
FROM Tbl_LoanRequests lr
LEFT JOIN Tbl_Policy p ON lr.PolicyId = p.Id
WHERE p.Id IS NULL
Select * From tbl_Policy;
Select * From Tbl_UserWallet;


    UPDATE Tbl_LoanInstallments
    SET DueDate = '2025-10-3'
    WHERE Id = 1 ;

	Delete from  Tbl_Users where id = 11;
		Delete from  Tbl_Users where id = 12;
			Delete from  Tbl_Users where id = 13;

	 UPDATE Tbl_UserWallet
    SET PolicyId = 5
    WHERE Id = 6 ;

Delete From Tbl_faq;

SELECT * FROM [__EFMigrationsHistory];

INSERT INTO Tbl_faq (Question, Answer, CreatedDate,IsActive) VALUES
('What is insurance and why do I need it?',
 'Insurance is a contract (policy) where an insurer provides financial protection against losses from an unexpected event. You need it to safeguard yourself and your family from financial hardship caused by events like accidents, illness, or property damage.',
 GETDATE(),1),

('What is the difference between a Premium and a Deductible?',
 'A ''Premium'' is the fixed amount you pay regularly (monthly or yearly) to keep your policy active. A ''Deductible'' is the initial amount you must pay out-of-pocket for a claim before the insurance company starts paying.',
 GETDATE(),1),

('How do I choose the right insurance policy?',
 'To choose the right policy, first assess your needs (e.g., family size, income, assets). Then, compare different plans for their coverage, benefits, exclusions, and premium. Always check the insurer''s claim settlement ratio.',
 GETDATE(),1),

('What is a ''nominee'' in a life insurance policy?',
 'A nominee is the person you appoint to receive the policy benefits (the death benefit) in the unfortunate event of your passing. It is crucial to appoint a nominee to ensure your family receives the funds smoothly.',
 GETDATE(),1),

('How do I file an insurance claim?',
 'The first step is to inform the insurance company about the incident (claim intimation) as soon as possible. Then, you''ll need to fill out a claim form and submit all the required documents. You can do this through our user dashboard.',
 GETDATE(),1),

('What documents are typically required for a claim?',
 'Documents vary by claim type. Generally, you need the filled claim form, policy document, and ID proof. For health claims, you need medical bills; for motor claims, a police report (FIR) might be needed; for death claims, the death certificate is required.',
 GETDATE(),1),

('What is No Claim Bonus (NCB) in motor insurance?',
 'No Claim Bonus (NCB) is a reward given by the insurer for not making any claims during a policy year. It is a significant discount on your renewal premium, which increases for every consecutive claim-free year.',
 GETDATE(),1),

('Can I have multiple insurance policies?',
 'Yes, you can absolutely have multiple insurance policies. For example, you can have separate policies for life, health, and motor insurance. You can even have multiple life or health insurance policies from different companies.',
 GETDATE(),1);


 Select * From Tbl_Category where Status = 0;
 
 
 
 SELECT 'sqlserver' dbms,t.TABLE_CATALOG,t.TABLE_SCHEMA,t.TABLE_NAME,c.COLUMN_NAME,c.ORDINAL_POSITION,c.DATA_TYPE,c.CHARACTER_MAXIMUM_LENGTH,n.CONSTRAINT_TYPE,k2.TABLE_SCHEMA,k2.TABLE_NAME,k2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLES t LEFT JOIN INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_CATALOG=c.TABLE_CATALOG AND t.TABLE_SCHEMA=c.TABLE_SCHEMA AND t.TABLE_NAME=c.TABLE_NAME LEFT JOIN(INFORMATION_SCHEMA.KEY_COLUMN_USAGE k JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS n ON k.CONSTRAINT_CATALOG=n.CONSTRAINT_CATALOG AND k.CONSTRAINT_SCHEMA=n.CONSTRAINT_SCHEMA AND k.CONSTRAINT_NAME=n.CONSTRAINT_NAME LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS r ON k.CONSTRAINT_CATALOG=r.CONSTRAINT_CATALOG AND k.CONSTRAINT_SCHEMA=r.CONSTRAINT_SCHEMA AND k.CONSTRAINT_NAME=r.CONSTRAINT_NAME)ON c.TABLE_CATALOG=k.TABLE_CATALOG AND c.TABLE_SCHEMA=k.TABLE_SCHEMA AND c.TABLE_NAME=k.TABLE_NAME AND c.COLUMN_NAME=k.COLUMN_NAME LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE k2 ON k.ORDINAL_POSITION=k2.ORDINAL_POSITION AND r.UNIQUE_CONSTRAINT_CATALOG=k2.CONSTRAINT_CATALOG AND r.UNIQUE_CONSTRAINT_SCHEMA=k2.CONSTRAINT_SCHEMA AND r.UNIQUE_CONSTRAINT_NAME=k2.CONSTRAINT_NAME WHERE t.TABLE_TYPE='BASE TABLE';