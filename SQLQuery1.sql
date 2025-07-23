use PolicyPro360;

Select * From Tbl_LoanInstallments;

UPDATE Tbl_LoanInstallments
    SET DueDate = '2025-07-23'
    WHERE Id = 6;
