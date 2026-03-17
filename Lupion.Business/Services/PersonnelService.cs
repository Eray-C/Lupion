using AutoMapper;
using Lupion.Business.Common;
using Lupion.Business.DTOs.Personnel;
using Lupion.Business.Exceptions;
using Lupion.Business.Requests.Personnel;
using Lupion.Data;
using Lupion.Data.Entities.PersonnelEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Lupion.Business.Services;

public class PersonnelService(DBContext context, DBContextFactory contextFactory, IMapper mapper, IHttpContextAccessor httpContextAccessor, ManagementDBContext managementContext)
    : BaseService(httpContextAccessor)
{

    public async Task<IEnumerable<PersonnelDTO>> GetPersonnelsAsync()
    {
        var entities = await context.Personnels
            .Include(p => p.GenderType)
            .Include(p => p.MaritalStatusType)
            .Include(p => p.DepartmentType)
            .Include(p => p.PersonnelType)
            .Include(p => p.StatusType)
            .AsNoTracking()
            .ToListAsync();


        var dtos = mapper.Map<IEnumerable<PersonnelDTO>>(entities);
        dtos.ToList().ForEach(dto => dto.Photo = null);
        return dtos;
    }

    public async Task<PersonnelDTO> GetPersonnelByIdAsync(int id)
    {
        var entity = await context.Personnels
           .Include(p => p.GenderType)
           .Include(p => p.MaritalStatusType)
           .Include(p => p.DepartmentType)
           .Include(p => p.PersonnelType)
           .Include(p => p.StatusType)
           .Where(x => x.Id == id)
           .AsNoTracking()
           .FirstOrDefaultAsync();

        return mapper.Map<PersonnelDTO>(entity);
    }

    public async Task<int> AddAsync(PersonnelRequest request)
    {
        var entity = mapper.Map<Personnel>(request);

        await context.AddAsync(entity);

        _ = await context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateAsync(int id, PersonnelRequest request)
    {
        var existingEntity = await context.Personnels.FindAsync(id) ?? throw new RecordNotFoundException();

        mapper.Map(request, existingEntity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Personnels.FindAsync(id) ?? throw new RecordNotFoundException();

        entity.IsDeleted = true;

        await context.SaveChangesAsync();
    }


    public async Task<IEnumerable<PersonnelLicenseDTO>> GetPersonnelLicenses(int personnelId)
    {
        await using var context = contextFactory.CreateDBContext();

        var result = await context.PersonnelLicenses
            .AsNoTracking()
            .Include(p => p.LicenseType)
            .Where(x => x.PersonnelId == personnelId)
            .ToListAsync();

        return mapper.Map<IEnumerable<PersonnelLicenseDTO>>(result);
    }

    public async Task<int> AddLicenseAsync(PersonnelLicenseRequest request)
    {
        var entity = mapper.Map<PersonnelLicense>(request);

        await context.AddAsync(entity);

        _ = await context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateLicenseAsync(int id, PersonnelLicenseRequest request)
    {
        var existingEntity = await context.PersonnelLicenses.FindAsync(id) ?? throw new RecordNotFoundException();

        mapper.Map(request, existingEntity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteLicenseAsync(int id)
    {
        var entity = await context.PersonnelLicenses.FindAsync(id) ?? throw new RecordNotFoundException();

        entity.IsDeleted = true;

        await context.SaveChangesAsync();
    }


    public async Task<IEnumerable<PersonnelSalaryDTO>> GetPersonnelSalaries(int personnelId)
    {
        await using var context = contextFactory.CreateDBContext();

        var result = await context.PersonnelSalaries
            .AsNoTracking()
            .Include(p => p.Currency)
            .Include(x => x.PaymentType)
            .Where(x => x.PersonnelId == personnelId)
            .ToListAsync();

        return mapper.Map<IEnumerable<PersonnelSalaryDTO>>(result);
    }

    public async Task<int> AddSalaryAsync(PersonnelSalaryRequest request)
    {
        var entity = mapper.Map<PersonnelSalary>(request);

        await context.AddAsync(entity);

        _ = await context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateSalaryAsync(int id, PersonnelSalaryRequest request)
    {
        var existingEntity = await context.PersonnelSalaries.FindAsync(id) ?? throw new RecordNotFoundException();

        mapper.Map(request, existingEntity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteSalaryAsync(int id)
    {
        var entity = await context.PersonnelSalaries.FindAsync(id) ?? throw new RecordNotFoundException();

        entity.IsDeleted = true;

        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PersonnelBonusDTO>> GetPersonnelBonuses(int personnelId)
    {
        await using var context = contextFactory.CreateDBContext();
        var result = await context.PersonnelBonuses
            .AsNoTracking()
            .Include(b => b.Type)
            .Include(b => b.Currency)
            .Where(x => x.PersonnelId == personnelId && !x.IsDeleted)
            .ToListAsync();
        return mapper.Map<IEnumerable<PersonnelBonusDTO>>(result);
    }

    public async Task<int> AddBonusAsync(PersonnelBonusRequest request)
    {
        var entity = mapper.Map<PersonnelBonus>(request);
        await context.AddAsync(entity);
        _ = await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateBonusAsync(int id, PersonnelBonusRequest request)
    {
        var existing = await context.PersonnelBonuses.FindAsync(id) ?? throw new RecordNotFoundException();
        mapper.Map(request, existing);
        await context.SaveChangesAsync();
    }

    public async Task DeleteBonusAsync(int id)
    {
        var entity = await context.PersonnelBonuses.FindAsync(id) ?? throw new RecordNotFoundException();
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PersonnelDeductionDTO>> GetPersonnelDeductions(int personnelId)
    {
        await using var context = contextFactory.CreateDBContext();
        var result = await context.PersonnelDeductions
            .AsNoTracking()
            .Include(d => d.Type)
            .Include(d => d.Currency)
            .Where(x => x.PersonnelId == personnelId && !x.IsDeleted)
            .ToListAsync();
        return mapper.Map<IEnumerable<PersonnelDeductionDTO>>(result);
    }

    public async Task<int> AddDeductionAsync(PersonnelDeductionRequest request)
    {
        var entity = mapper.Map<PersonnelDeduction>(request);
        await context.AddAsync(entity);
        _ = await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateDeductionAsync(int id, PersonnelDeductionRequest request)
    {
        var existing = await context.PersonnelDeductions.FindAsync(id) ?? throw new RecordNotFoundException();
        mapper.Map(request, existing);
        await context.SaveChangesAsync();
    }

    public async Task DeleteDeductionAsync(int id)
    {
        var entity = await context.PersonnelDeductions.FindAsync(id) ?? throw new RecordNotFoundException();
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PersonnelAdvanceDTO>> GetPersonnelAdvances(int personnelId)
    {
        await using var context = contextFactory.CreateDBContext();
        var result = await context.PersonnelAdvances
            .AsNoTracking()
            .Include(a => a.Currency)
            .Where(x => x.PersonnelId == personnelId && !x.IsDeleted)
            .ToListAsync();
        return mapper.Map<IEnumerable<PersonnelAdvanceDTO>>(result);
    }

    public async Task<int> AddAdvanceAsync(PersonnelAdvanceRequest request)
    {
        var entity = mapper.Map<PersonnelAdvance>(request);
        entity.RemainingAmount = entity.GivenAmount;
        await context.AddAsync(entity);
        _ = await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAdvanceAsync(int id, PersonnelAdvanceRequest request)
    {
        var existing = await context.PersonnelAdvances.FindAsync(id) ?? throw new RecordNotFoundException();
        mapper.Map(request, existing);
        if (existing.RemainingAmount == 0 && !existing.IsCompleted)
            existing.RemainingAmount = existing.GivenAmount;
        await context.SaveChangesAsync();
    }

    public async Task CloseAdvanceAsync(int id)
    {
        var advance = await context.PersonnelAdvances.FindAsync(id) ?? throw new RecordNotFoundException();
        advance.IsCompleted = true;
        advance.AdvanceClosedDate = DateTime.UtcNow.Date;
        advance.RemainingAmount = 0;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PersonnelAdvancePaymentDTO>> GetPersonnelAdvancePaymentsAsync(int personnelId)
    {
        await using var ctx = contextFactory.CreateDBContext();
        var list = await ctx.PersonnelAdvancePayments
            .AsNoTracking()
            .Where(x => x.PersonnelId == personnelId && !x.IsDeleted)
            .OrderByDescending(x => x.PaymentDate)
            .ThenByDescending(x => x.Id)
            .ToListAsync();
        return mapper.Map<IEnumerable<PersonnelAdvancePaymentDTO>>(list);
    }

    public async Task<IEnumerable<PersonnelPaymentHistoryItemDTO>> GetPersonnelPaymentHistorySummaryAsync(int personnelId)
    {
        await using var ctx = contextFactory.CreateDBContext();
        var list = await ctx.PersonnelPaymentHistory
            .AsNoTracking()
            .Where(h => h.PersonnelId == personnelId && !h.IsDeleted)
            .OrderByDescending(h => h.PeriodYear)
            .ThenByDescending(h => h.PeriodMonth)
            .ToListAsync();
        return list.Select(h => new PersonnelPaymentHistoryItemDTO
        {
            Id = h.Id,
            PeriodYear = h.PeriodYear,
            PeriodMonth = h.PeriodMonth,
            MonthName = TurkishMonthHelper.GetName(h.PeriodMonth),
            RealizedDate = h.RealizedDate,
            NetSalary = h.NetSalary,
            TravelExpense = h.TravelExpense,
            BonusTotal = h.BonusTotal,
            DeductionTotal = h.DeductionTotal,
            AdvanceDeductionTotal = h.AdvanceDeductionTotal,
            TotalPayable = h.TotalPaid,
            Notes = h.Notes
        }).ToList();
    }

    public async Task RecordAdvancePaymentAsync(PersonnelAdvancePaymentRequest request)
    {
        var advance = await context.PersonnelAdvances.FindAsync(request.PersonnelAdvanceId) ?? throw new RecordNotFoundException();
        if (advance.PersonnelId != request.PersonnelId) throw new RecordNotFoundException();
        if (advance.IsCompleted) throw new InvalidOperationException("Bu avans zaten kapatÄ±lmÄ±ÅŸ.");
        var amount = request.Amount <= advance.RemainingAmount ? request.Amount : advance.RemainingAmount;
        if (amount <= 0) return;

        var payment = mapper.Map<PersonnelAdvancePayment>(request);
        payment.Amount = amount;
        payment.PaymentDate = new DateTime(request.PeriodYear, request.PeriodMonth, 1);
        await context.PersonnelAdvancePayments.AddAsync(payment);

        advance.RemainingAmount -= amount;
        if (advance.RemainingAmount <= 0)
        {
            advance.IsCompleted = true;
            advance.AdvanceClosedDate = DateTime.UtcNow.Date;
            advance.RemainingAmount = 0;
        }
        await context.SaveChangesAsync();
    }

    public async Task DeleteAdvanceAsync(int id)
    {
        var entity = await context.PersonnelAdvances.FindAsync(id) ?? throw new RecordNotFoundException();
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PersonnelContactDTO>> GetPersonnelContacts(int personnelId)
    {
        await using var context = contextFactory.CreateDBContext();

        var result = await context.PersonnelContacts
            .AsNoTracking()
            .Where(x => x.PersonnelId == personnelId && !x.IsDeleted)
            .ToListAsync();

        return mapper.Map<IEnumerable<PersonnelContactDTO>>(result);
    }

    public async Task<int> AddContactAsync(PersonnelContactRequest request)
    {
        var entity = mapper.Map<PersonnelContact>(request);

        await context.AddAsync(entity);

        _ = await context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateContactAsync(int id, PersonnelContactRequest request)
    {
        var existingEntity = await context.PersonnelContacts.FindAsync(id) ?? throw new RecordNotFoundException();

        mapper.Map(request, existingEntity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(int id)
    {
        var entity = await context.PersonnelContacts.FindAsync(id) ?? throw new RecordNotFoundException();

        entity.IsDeleted = true;

        await context.SaveChangesAsync();
    }




    public async Task<IEnumerable<PersonnelRelativeContactDTO>> GetPersonnelRelativeContacts(int personnelId)
    {
        await using var context = contextFactory.CreateDBContext();

        var entities = await context.PersonnelRelativeContacts
            .AsNoTracking()
            .Include(x => x.GenderType)
            .Include(x => x.RelationshipType)
            .Where(x => x.PersonnelId == personnelId)
            .ToListAsync();

        return mapper.Map<IEnumerable<PersonnelRelativeContactDTO>>(entities);
    }

    public async Task<int> AddRelativeContactAsync(PersonnelRelativeContactRequest request)
    {
        var entity = mapper.Map<PersonnelRelativeContact>(request);

        await context.AddAsync(entity);

        _ = await context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateRelativeContactAsync(int id, PersonnelRelativeContactRequest request)
    {
        var existingEntity = await context.PersonnelRelativeContacts.FindAsync(id) ?? throw new RecordNotFoundException();

        mapper.Map(request, existingEntity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteRelativeContactAsync(int id)
    {
        var entity = await context.PersonnelRelativeContacts.FindAsync(id) ?? throw new RecordNotFoundException();

        entity.IsDeleted = true;

        await context.SaveChangesAsync();
    }

    public async Task<PersonnelAllDataDTO> GetPersonnelAllDataAsync(int personnelId)
    {
        var relativesTask = GetPersonnelRelativeContacts(personnelId);
        var contactsTask = GetPersonnelContacts(personnelId);
        var salariesTask = GetPersonnelSalaries(personnelId);
        var bonusesTask = GetPersonnelBonuses(personnelId);
        var deductionsTask = GetPersonnelDeductions(personnelId);
        var advancesTask = GetPersonnelAdvances(personnelId);
        var paymentHistoryTask = GetPersonnelPaymentHistorySummaryAsync(personnelId);
        var licencesTask = GetPersonnelLicenses(personnelId);
        var personnelTask = GetPersonnelByIdAsync(personnelId);

        await Task.WhenAll(relativesTask, contactsTask, salariesTask, bonusesTask, deductionsTask, advancesTask, paymentHistoryTask, licencesTask, personnelTask);

        return new PersonnelAllDataDTO
        {
            Relatives = await relativesTask,
            Contacts = await contactsTask,
            Salaries = await salariesTask,
            Bonuses = await bonusesTask,
            Deductions = await deductionsTask,
            Advances = await advancesTask,
            PaymentHistory = await paymentHistoryTask,
            Licences = await licencesTask,
            Personnel = await personnelTask
        };
    }
    public async Task<string?> GetPersonnelPhoto(int personnelId)
    {
        var result = await context.Personnels.FindAsync(personnelId);
        if (result == null || result.Photo == null || result.Photo.Length == 0)
            return null;

        return Convert.ToBase64String(result.Photo);
    }
    public async Task<IEnumerable<PersonnelPayrollDTO>> GetMonthlyPayrollAsync(int year, int month)
    {
        return await context.Database
            .SqlQueryRaw<PersonnelPayrollDTO>(
                "EXEC dbo.GetMonthlyPersonnelPayroll @Year, @Month",
                new SqlParameter("@Year", year),
                new SqlParameter("@Month", month)
            )
            .ToListAsync();
    }


    private async Task<string> GetPersonnelFullNameAsync(int personnelId)
    {
        var p = await context.Personnels
            .AsNoTracking()
            .Where(x => x.Id == personnelId)
            .Select(x => new { x.FirstName, x.LastName })
            .FirstOrDefaultAsync();
        return p != null ? $"{p.FirstName} {p.LastName}".Trim() : string.Empty;
    }

    public async Task SaveMonthlyPayrollAsync(PersonnelPayrollBatchRequest request)
    {
        if (request?.Payrolls == null)
            return;

        var periodStart = new DateTime(request.Year, request.Month, 1);
        foreach (var payroll in request.Payrolls)
        {
            var existingPayroll = payroll.Id.HasValue
                ? await context.PersonnelPayrolls.FirstOrDefaultAsync(p => p.Id == payroll.Id.Value)
                : await context.PersonnelPayrolls.FirstOrDefaultAsync(p =>
                    p.PersonnelId == payroll.PersonnelId && p.PeriodYear == request.Year && p.PeriodMonth == request.Month);

            if (existingPayroll is null)
            {
                var newPayroll = mapper.Map<PersonnelPayroll>(payroll);
                newPayroll.PeriodYear = request.Year;
                newPayroll.PeriodMonth = request.Month;
                newPayroll.EffectiveDate = payroll.EffectiveDate ?? periodStart;
                newPayroll.PersonnelName = !string.IsNullOrEmpty(payroll.PersonnelName) ? payroll.PersonnelName : await GetPersonnelFullNameAsync(payroll.PersonnelId);
                newPayroll.Provider = !string.IsNullOrEmpty(payroll.Provider) ? payroll.Provider : "KaydedilmiÅŸ Ã‡alÄ±ÅŸma";
                newPayroll.Note = request.Note;
                newPayroll.CreatedBy = CurrentUser.Id;
                newPayroll.CreatedDate = DateTime.UtcNow;
                await context.PersonnelPayrolls.AddAsync(newPayroll);
            }
            else
            {
                mapper.Map(payroll, existingPayroll);
                existingPayroll.PeriodYear = request.Year;
                existingPayroll.PeriodMonth = request.Month;
                existingPayroll.EffectiveDate = payroll.EffectiveDate ?? periodStart;
                if (!string.IsNullOrEmpty(payroll.PersonnelName)) existingPayroll.PersonnelName = payroll.PersonnelName;
                if (!string.IsNullOrEmpty(payroll.Provider)) existingPayroll.Provider = payroll.Provider;
                existingPayroll.Note = request.Note;
                existingPayroll.UpdatedBy = CurrentUser.Id;
                existingPayroll.UpdatedDate = DateTime.UtcNow;
            }
        }

        await context.SaveChangesAsync();

        var payrollsInPeriod = await context.PersonnelPayrolls
            .Where(p => p.PeriodYear == request.Year && p.PeriodMonth == request.Month && !p.IsDeleted)
            .ToListAsync();

        foreach (var payroll in payrollsInPeriod)
        {
            var openAdvances = await context.PersonnelAdvances
                .Where(a => a.PersonnelId == payroll.PersonnelId && !a.IsDeleted && !a.IsCompleted
                    && a.StartDeductionDate <= periodStart)
                .ToListAsync();

            foreach (var advance in openAdvances)
            {
                var alreadyPaid = await context.PersonnelAdvancePayments
                    .AnyAsync(x => x.PersonnelAdvanceId == advance.Id && x.PeriodYear == request.Year && x.PeriodMonth == request.Month && !x.IsDeleted);
                if (alreadyPaid) continue;

                var amount = advance.DeductionAmountPerMonth <= advance.RemainingAmount
                    ? advance.DeductionAmountPerMonth
                    : advance.RemainingAmount;
                if (amount <= 0) continue;

                var payment = new PersonnelAdvancePayment
                {
                    PersonnelId = payroll.PersonnelId,
                    PersonnelAdvanceId = advance.Id,
                    Amount = amount,
                    PaymentDate = periodStart,
                    PeriodYear = request.Year,
                    PeriodMonth = request.Month,
                    PersonnelPayrollId = payroll.Id
                };
                await context.PersonnelAdvancePayments.AddAsync(payment);

                advance.RemainingAmount -= amount;
                if (advance.RemainingAmount <= 0)
                {
                    advance.IsCompleted = true;
                    advance.AdvanceClosedDate = DateTime.UtcNow.Date;
                    advance.RemainingAmount = 0;
                }
            }
        }

        await context.SaveChangesAsync();

        var payrollPeriod = await context.PaidPayrolls.FirstOrDefaultAsync(x => x.Year == request.Year && x.Month == request.Month && !x.IsDeleted);
        if (payrollPeriod == null)
        {
            payrollPeriod = new PaidPayroll
            {
                Year = request.Year,
                Month = request.Month,
                Note = request.Note,
                CreatedBy = CurrentUser.Id,
                CreatedDate = DateTime.UtcNow
            };
            await context.PaidPayrolls.AddAsync(payrollPeriod);
        }
        else
        {
            payrollPeriod.Note = request.Note;
            payrollPeriod.UpdatedBy = CurrentUser.Id;
            payrollPeriod.UpdatedDate = DateTime.UtcNow;
        }
        await context.SaveChangesAsync();
    }

    public async Task MarkPayrollAsRealizedAsync(int personnelPayrollId)
    {
        var payroll = await context.PersonnelPayrolls.FindAsync(personnelPayrollId) ?? throw new RecordNotFoundException();
        await AddPaymentHistoryIfNotExistsAsync(payroll, DateTime.UtcNow, null);
        await context.SaveChangesAsync();
    }

    public async Task SoftDeletePeriodPayrollsAsync(int year, int month)
    {
        var payrolls = await context.PersonnelPayrolls
            .Where(p => p.PeriodYear == year && p.PeriodMonth == month && !p.IsDeleted)
            .ToListAsync();
        foreach (var p in payrolls)
            p.IsDeleted = true;
        var payrollPeriod = await context.PaidPayrolls.FirstOrDefaultAsync(x => x.Year == year && x.Month == month && !x.IsDeleted);
        if (payrollPeriod != null)
            payrollPeriod.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PayrollPeriodListDTO>> GetPayrollPeriodsAsync()
    {
        var list = await context.PaidPayrolls
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .ToListAsync();

        var userIds = list.Select(x => x.UpdatedBy ?? x.CreatedBy).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToList();
        var userNames = new Dictionary<int, string>();
        foreach (var id in userIds)
        {
            var user = await managementContext.Users.AsNoTracking()
                .Where(u => u.Id == id && !u.IsDeleted)
                .Select(u => new { u.FirstName, u.LastName })
                .FirstOrDefaultAsync();
            if (user != null)
                userNames[id] = $"{user.FirstName} {user.LastName}".Trim();
        }

        return mapper.Map<List<PayrollPeriodListDTO>>(list, opt => opt.Items["UserNames"] = userNames);
    }

    public async Task MarkAllPayrollsInPeriodAsRealizedAsync(PersonnelPayrollRealizeRequest request)
    {
        var payrolls = await context.PersonnelPayrolls
            .Where(p => p.PeriodYear == request.Year && p.PeriodMonth == request.Month && !p.IsDeleted)
            .ToListAsync();
        if (payrolls.Count == 0)
            throw new InvalidOperationException("Bu dÃ¶nem iÃ§in bordro onaylanmamÄ±ÅŸ. Ã–nce bordroyu onaylayÄ±n.");
        var existingHistory = await context.PersonnelPaymentHistory
            .Where(h => h.PeriodYear == request.Year && h.PeriodMonth == request.Month && !h.IsDeleted)
            .ToListAsync();
        foreach (var h in existingHistory)
            h.IsDeleted = true;
        foreach (var payroll in payrolls)
            await AddPaymentHistoryIfNotExistsAsync(payroll, request.RealizedDate, request.Notes);
        await context.SaveChangesAsync();

        var totalPaid = await context.PersonnelPaymentHistory
            .Where(h => !h.IsDeleted && h.PeriodYear == request.Year && h.PeriodMonth == request.Month)
            .SumAsync(h => h.TotalPaid ?? 0);
        var payrollPeriod = await context.PaidPayrolls.FirstOrDefaultAsync(x => x.Year == request.Year && x.Month == request.Month && !x.IsDeleted);
        if (payrollPeriod == null)
        {
            payrollPeriod = new PaidPayroll
            {
                Year = request.Year,
                Month = request.Month,
                PaymentNote = request.Notes,
                TotalPaidAmount = totalPaid,
                CreatedBy = CurrentUser.Id,
                CreatedDate = DateTime.UtcNow
            };
            await context.PaidPayrolls.AddAsync(payrollPeriod);
        }
        else
        {
            payrollPeriod.PaymentNote = request.Notes;
            payrollPeriod.TotalPaidAmount = totalPaid;
            payrollPeriod.UpdatedBy = CurrentUser.Id;
            payrollPeriod.UpdatedDate = DateTime.UtcNow;
        }
        await context.SaveChangesAsync();
    }

    private async Task AddPaymentHistoryIfNotExistsAsync(PersonnelPayroll payroll, DateTime realizedDate, string? notes)
    {
        var alreadyExists = await context.PersonnelPaymentHistory
            .AnyAsync(h => h.PersonnelPayrollId == payroll.Id && !h.IsDeleted);
        if (alreadyExists) return;

        var bonusSum = await context.PersonnelBonuses
            .Where(b => b.PersonnelId == payroll.PersonnelId && b.Year == payroll.PeriodYear && b.Month == payroll.PeriodMonth && !b.IsDeleted)
            .SumAsync(b => b.Amount);
        var deductionSum = await context.PersonnelDeductions
            .Where(d => d.PersonnelId == payroll.PersonnelId && d.Year == payroll.PeriodYear && d.Month == payroll.PeriodMonth && !d.IsDeleted)
            .SumAsync(d => d.Amount);
        var advanceSum = await context.PersonnelAdvancePayments
            .Where(a => a.PersonnelId == payroll.PersonnelId && a.PeriodYear == payroll.PeriodYear && a.PeriodMonth == payroll.PeriodMonth && !a.IsDeleted)
            .SumAsync(a => a.Amount);

        var totalPaid = (payroll.NetSalary ?? 0) + (payroll.TravelExpense ?? 0) + bonusSum - deductionSum - advanceSum;

        var history = new PersonnelPaymentHistory
        {
            PersonnelId = payroll.PersonnelId,
            PersonnelPayrollId = payroll.Id,
            PeriodYear = payroll.PeriodYear,
            PeriodMonth = payroll.PeriodMonth,
            RealizedDate = realizedDate,
            Notes = notes,
            NetSalary = payroll.NetSalary,
            TravelExpense = payroll.TravelExpense,
            BonusTotal = bonusSum,
            DeductionTotal = deductionSum,
            AdvanceDeductionTotal = advanceSum,
            TotalPaid = totalPaid
        };
        await context.PersonnelPaymentHistory.AddAsync(history);
    }

    public async Task<IEnumerable<PersonnelDTO>> GetDriversAsync()
    {
        var records = await context.Personnels
            .Where(p =>
                p.PersonnelType != null &&
                p.PersonnelType.Name == "SÃ¼rÃ¼cÃ¼"
            )
            .ToListAsync();

        var result = mapper.Map<IEnumerable<PersonnelDTO>>(records);

        return result;
    }


}
