

function printPolicyDetails() {
    showNotification('Preparing policy details for printing...', 'info');
    
    // Get the left column content
    var leftColumn = document.querySelector('.col-lg-8 .card');
    
    if (leftColumn) {
        // Create a new window for printing
        var printWindow = window.open('', '_blank', 'width=800,height=600');
        
        // Get the content and create a clean version for printing
        var content = leftColumn.innerHTML;
        
        // Create the HTML for printing
        var printHTML = '<!DOCTYPE html>' +
            '<html>' +
            '<head>' +
            '<title>Policy Details - PolicyPro360</title>' +
            '<style>' +
            'body { margin: 0; padding: 30px; font-family: "Segoe UI", Arial, sans-serif; background: white; color: #333; }' +
            '.card { border: 2px solid #f5a526; border-radius: 12px; padding: 30px; background: white; box-shadow: 0 4px 8px rgba(0,0,0,0.1); }' +
            '.policy-header { border-bottom: 3px solid #f5a526; padding-bottom: 20px; margin-bottom: 30px; text-align: center; }' +
            '.policy-header h4 { color: #f5a526; font-size: 28px; font-weight: bold; margin: 0 0 10px 0; }' +
            '.policy-header small { color: #666; font-size: 16px; }' +
            '.badge { background: linear-gradient(135deg, #28c76f, #20c997); color: white; padding: 8px 20px; border-radius: 25px; font-size: 14px; font-weight: bold; text-transform: uppercase; letter-spacing: 1px; margin-top: 15px; display: inline-block; box-shadow: 0 2px 4px rgba(40, 199, 111, 0.3); }' +
            'h5 { color: #f5a526; font-size: 22px; font-weight: bold; margin: 25px 0 20px 0; border-left: 4px solid #f5a526; padding-left: 15px; }' +
            '.row { display: flex; flex-wrap: wrap; margin: 0 -10px; }' +
            '.col-md-4, .col-md-6 { padding: 0 10px; margin-bottom: 15px; }' +
            '.col-md-4 { flex: 0 0 33.333%; }' +
            '.col-md-6 { flex: 0 0 50%; }' +
            '.info-item { display: flex; align-items: flex-start; padding: 15px; border: 1px solid #e9ecef; border-radius: 8px; background: #f8f9fa; margin-bottom: 10px; transition: all 0.3s ease; }' +
            '.info-item:hover { background: #e9ecef; transform: translateY(-2px); box-shadow: 0 4px 8px rgba(0,0,0,0.1); }' +
            '.info-icon { color: #f5a526; font-size: 24px; margin-right: 15px; min-width: 30px; text-align: center; background: rgba(245, 165, 38, 0.1); padding: 8px; border-radius: 50%; }' +
            '.info-content { flex: 1; }' +
            '.info-label { display: block; color: #6c757d; font-size: 13px; font-weight: 500; text-transform: uppercase; letter-spacing: 0.5px; margin-bottom: 5px; }' +
            '.info-value { display: block; color: #2c3e50; font-weight: 700; font-size: 16px; line-height: 1.4; }' +
            '.document-header { text-align: center; margin-bottom: 40px; padding: 20px; background: linear-gradient(135deg, #f5a526, #ff6b35); color: white; border-radius: 10px; }' +
            '.document-header h1 { margin: 0; font-size: 32px; font-weight: bold; }' +
            '.document-header p { margin: 5px 0 0 0; font-size: 16px; opacity: 0.9; }' +
            '.document-footer { text-align: center; margin-top: 40px; padding: 20px; border-top: 2px solid #f5a526; color: #666; font-size: 14px; }' +
            '@media print {' +
            '    body { margin: 15px; }' +
            '    .card { box-shadow: none; border: 2px solid #f5a526; }' +
            '    .info-item:hover { transform: none; box-shadow: none; }' +
            '    .document-header { background: #f5a526 !important; -webkit-print-color-adjust: exact; }' +
            '    .badge { background: #28c76f !important; -webkit-print-color-adjust: exact; color: white !important; box-shadow: none !important; }' +
            '    .info-icon { background: rgba(245, 165, 38, 0.1) !important; -webkit-print-color-adjust: exact; }' +
            '}' +
            '</style>' +
            '</head>' +
            '<body>' +
            '<div class="document-header">' +
            '    <h1>PolicyPro360</h1>' +
            '    <p>Policy Details Document</p>' +
            '</div>' +
            content +
            '<div class="document-footer">' +
            '    <p><strong>Generated on:</strong> ' + new Date().toLocaleDateString() + ' | <strong>PolicyPro360</strong> - Your Policy, Our Priority</p>' +
            '</div>' +
            '<script>' +
            'window.onload = function() {' +
            '    window.print();' +
            '    window.onafterprint = function() {' +
            '        window.close();' +
            '    };' +
            '};' +
            '</script>' +
            '</body>' +
            '</html>';
        
        printWindow.document.write(printHTML);
        printWindow.document.close();
        showNotification('Policy details printed successfully!', 'success');
    } else {
        showNotification('Could not find policy details to print', 'error');
    }
}

function showNotification(message, type) {
    var notification = document.createElement('div');
    notification.className = 'alert alert-' + (type === 'success' ? 'success' : 'info') + ' alert-dismissible fade show position-fixed';
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    notification.innerHTML = '<i class="ti ti-' + (type === 'success' ? 'check-circle' : 'info-circle') + ' me-2"></i>' +
        message +
        '<button type="button" class="btn-close" data-bs-dismiss="alert"></button>';

    document.body.appendChild(notification);

    setTimeout(function() {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 3000);
} 